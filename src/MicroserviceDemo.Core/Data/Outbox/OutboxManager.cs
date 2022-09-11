using System;
using System.Collections.Generic;
using System.Text;
using Core.Configuration;
using Core.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Data.Outbox
{
	public class OutboxManager<TContext> : IOutboxManager
        where TContext : DataContext
	{
		private readonly IMessageBroker _messageBroker;
        private readonly IRepositoryScopeFactory<TContext> _serviceScopeFactory;
        private List<OutboxItem> _changeList = new();

        public OutboxManager(
            IMessageBroker messageBroker,
            IRepositoryScopeFactory<TContext> scopeFactory)
        {
            _messageBroker = messageBroker;
            _serviceScopeFactory = scopeFactory;
        }

        public async Task<List<OutboxItem>> GetUnsentItemsAsync()
        {
            List<OutboxItem> unsent = new();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.GetRepository<OutboxItem>();
                unsent = await repository.Find(p => p.State == 0).ToListAsync();
            }
            return unsent;
        }

        public async Task SendPendingItemsAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.GetRepository<OutboxItem>();

                List<OutboxItem> unsentOutboxList = await repository.Find(p => p.State == 0).ToListAsync();
                unsentOutboxList.ForEach(outboxItem =>
                {
                    _messageBroker.Publish(Encoding.UTF8.GetBytes(outboxItem.Data), outboxItem.Type);
                    outboxItem.Send();
                    _changeList.Add(outboxItem);
                });

                await repository.UpdateRangeAsync(_changeList);
                _changeList.Clear();
            }
        }
    }
}

