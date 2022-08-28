using System;
using Core.Data;
using Core.Events;
using MediatR;
using Orders.Domain;

namespace Orders.EventListener.Events
{
    public class CustomerCreatedEventHandler : INotificationHandler<DomainEventNotification<CustomerCreatedEvent>>
    {
        private readonly IRepository<Buyer> _repository;

        public CustomerCreatedEventHandler(IRepository<Buyer> repository)
        {
            _repository = repository;
        }

        public async Task Handle(DomainEventNotification<CustomerCreatedEvent> notification, CancellationToken cancellationToken)
        {
            CustomerCreatedEvent eventData = notification.DomainEvent;

            await _repository.AddAsync(Buyer.FromCustomer(eventData.CustomerId, eventData.FullName));
        }
    }
}

