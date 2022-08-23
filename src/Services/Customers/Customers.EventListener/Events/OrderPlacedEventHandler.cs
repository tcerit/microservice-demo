using System;
using Ardalis.GuardClauses;
using Core.Data;
using Core.Events;
using Customers.Domain;
using MediatR;

namespace Customers.EventListener.Events
{
	public class OrderPlacedEventHandler : INotificationHandler<DomainEventNotification<OrderPlacedEvent>>
	{
        private readonly IRepository<Customer> _repository;
        public OrderPlacedEventHandler(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task Handle(DomainEventNotification<OrderPlacedEvent> notification, CancellationToken cancellationToken)
        {
            OrderPlacedEvent eventData = notification.DomainEvent;

            Customer customer = await _repository.GetByIdAsync(eventData.BuyerId);

            Guard.Against.Null(customer);

            customer.EarnPointsFromOrder(eventData.OrderTotal);
            
        }
    }
}

