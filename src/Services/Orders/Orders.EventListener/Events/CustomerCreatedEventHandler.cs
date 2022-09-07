using System;
using Core.Configuration;
using Core.Data;
using Core.Events;
using MediatR;
using Orders.Data;
using Orders.Domain;

namespace Orders.EventListener.Events
{
    public class CustomerCreatedEventHandler : INotificationHandler<DomainEventNotification<CustomerCreatedEvent>>
    {
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly IRepositoryScopeFactory<OrdersDataContext> _serviceScopeFactory;

        public CustomerCreatedEventHandler(
            IRepositoryScopeFactory<OrdersDataContext> serviceScopeFactory,
            ILogger<ProductCreatedEventHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task Handle(DomainEventNotification<CustomerCreatedEvent> notification, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var _repository = scope.GetRepository<Buyer>();

                    CustomerCreatedEvent eventData = notification.DomainEvent;

                    await _repository.AddAsync(Buyer.FromCustomer(eventData.CustomerId, eventData.FullName));
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}

