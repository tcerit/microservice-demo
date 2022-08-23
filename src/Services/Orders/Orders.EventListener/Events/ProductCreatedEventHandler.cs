using System;
using Core.Data;
using Core.Events;
using MediatR;
using Orders.Domain;

namespace Orders.EventListener.Events
{
	public class ProductCreatedEventHandler : INotificationHandler<DomainEventNotification<ProductCreatedEvent>>
    {
        private readonly IRepository<OrderProduct> _repository;

        public ProductCreatedEventHandler(IRepository<OrderProduct> repository)
        {
            _repository = repository;
        }

        public async Task Handle(DomainEventNotification<ProductCreatedEvent> notification, CancellationToken cancellationToken)
        {
            ProductCreatedEvent eventData = notification.DomainEvent;

            OrderProduct product = OrderProduct.FromProduct(eventData.ProductId, eventData.Name, eventData.Price);

            await _repository.AddAsync(product);
        }
    }
}

