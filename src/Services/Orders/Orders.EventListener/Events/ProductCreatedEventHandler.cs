using System;
using Core.Configuration;
using Core.Data;
using Core.Events;
using MediatR;
using Orders.Data;
using Orders.Domain;

namespace Orders.EventListener.Events
{
	public class ProductCreatedEventHandler : INotificationHandler<DomainEventNotification<ProductCreatedEvent>>
    {

        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly IRepositoryScopeFactory<OrdersDataContext> _serviceScopeFactory;

        public ProductCreatedEventHandler(IRepositoryScopeFactory<OrdersDataContext> serviceScopeFactory, ILogger<ProductCreatedEventHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task Handle(DomainEventNotification<ProductCreatedEvent> notification, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _repository = scope.GetRepository<OrderProduct>();

                try
                {
                    ProductCreatedEvent eventData = notification.DomainEvent;

                    OrderProduct product = OrderProduct.FromProduct(eventData.ProductId, eventData.Name, eventData.Price);

                    await _repository.AddAsync(product);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}

