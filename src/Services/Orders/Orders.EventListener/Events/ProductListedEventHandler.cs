using System;
using Core.Data;
using Core.Events;
using MediatR;
using Orders.Domain;

namespace Orders.EventListener.Events
{
    public class ProductListedEventHandler : INotificationHandler<DomainEventNotification<ProductListedEvent>>
    {
        private readonly IRepository<OrderProduct> _repository;

        public ProductListedEventHandler(IRepository<OrderProduct> repository)
        {
            _repository = repository;
        }

        public async Task Handle(DomainEventNotification<ProductListedEvent> notification, CancellationToken cancellationToken)
        {
            ProductListedEvent eventData = notification.DomainEvent;

            OrderProduct? product = await _repository.GetByIdAsync(eventData.ProductId);
            if (product != null)
            {
                product.MakeAvailable();
                await _repository.UpdateAsync(product);
            }
                

            
        }
    }
}

