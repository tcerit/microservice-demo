using System;
using Core.Domain;
using Core.Events;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Core.Data.Outbox;

namespace Core.Data
{
    public abstract class DataContext : DbContext
    {

        private readonly IDomainEventDispatcher _dispatcher;
        private Entity[] _domainEventEntities;

        public DbSet<OutboxItem> Outbox { get; set; }

        public DataContext(DbContextOptions options, IDomainEventDispatcher dispatcher) : base(options)
        {
            _dispatcher = dispatcher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            _preSaveChanges();
            var res = base.SaveChanges();
            _dispatchDomainEvents().GetAwaiter().GetResult();
            return res;
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _preSaveChanges();
            var res = await base.SaveChangesAsync(cancellationToken);
            await _dispatchDomainEvents();
            return res;
        }

        private void _preSaveChanges()
        {
            _domainEventEntities = ChangeTracker.Entries<Entity>()
                .Select(po => po.Entity)
                .Where(po => po.DomainEvents.Any())
                .ToArray();
        }

        private async Task _dispatchDomainEvents()
        {


            foreach (var entity in _domainEventEntities)
            {
                IDomainEvent dev;
                while (entity.DomainEvents.TryTake(out dev))
                {
                    await _dispatcher.Dispatch(dev);
                    await SaveEventToOutbox(dev);
                }
            }
        }

        protected virtual async Task SaveEventToOutbox(IDomainEvent domainEvent)
        {
            await base.AddAsync(OutboxItem.FromEvent(domainEvent));
        }



    }
}

