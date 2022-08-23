using System;
using Core.Data;
using Core.Events;
using MediatR;
using Orders.Domain;

namespace Orders.EventListener.Events
{
	public class ProductDelistedEventHandler : INotificationHandler<DomainEventNotification<ProductDelistedEvent>>
    {

        private readonly IRepository<OrderProduct> _repository;

        public ProductDelistedEventHandler(IRepository<OrderProduct> repository)
        {
            _repository = repository;
        }

        public async Task Handle(DomainEventNotification<ProductDelistedEvent> notification, CancellationToken cancellationToken)
        {
            ProductDelistedEvent eventData = notification.DomainEvent;

            OrderProduct? product = await _repository.GetByIdAsync(eventData.ProductId);
            if (product != null)
            {
                product.MakeUnavailable();

                await _repository.UpdateAsync(product);
            }

            
        }
    }
}

