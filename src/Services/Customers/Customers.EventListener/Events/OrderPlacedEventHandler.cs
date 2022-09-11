using System;
using Ardalis.GuardClauses;
using Core.Configuration;
using Core.Data;
using Core.Events;
using Customers.Data;
using Customers.Domain;
using MediatR;

namespace Customers.EventListener.Events
{
	public class OrderPlacedEventHandler : INotificationHandler<DomainEventNotification<OrderPlacedEvent>>
	{
        private readonly ILogger<OrderPlacedEventHandler> _logger;
        private readonly IRepositoryScopeFactory<CustomersDataContext> _serviceScopeFactory;

        public OrderPlacedEventHandler(
            ILogger<OrderPlacedEventHandler> logger,
            IRepositoryScopeFactory<CustomersDataContext> serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Handle(DomainEventNotification<OrderPlacedEvent> notification, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var repository = scope.GetRepository<Customer>();

                    OrderPlacedEvent eventData = notification.DomainEvent;
                    Customer? customer = await repository.GetByIdAsync(eventData.BuyerId);
                    Guard.Against.Null(customer);
                    customer.EarnPointsFromOrder(eventData.OrderTotal);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            

            

            

            
            
        }
    }
}

