using System;
using Core.Configuration;
using Core.Events;
using MediatR;
using Orders.Data;
using Orders.Domain;

namespace Orders.EventListener.Events
{
    public class ProductPriceUpdatedEventHandler : INotificationHandler<DomainEventNotification<ProductPriceUpdatedEvent>>
    {

        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly IRepositoryScopeFactory<OrdersDataContext> _serviceScopeFactory;

        public ProductPriceUpdatedEventHandler(
            IRepositoryScopeFactory<OrdersDataContext> serviceScopeFactory,
            ILogger<ProductCreatedEventHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task Handle(DomainEventNotification<ProductPriceUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var repository = scope.GetRepository<OrderProduct>();

                    ProductPriceUpdatedEvent eventData = notification.DomainEvent;

                    OrderProduct? product = await repository.GetByIdAsync(eventData.ProductId);

                    if (product != null)
                    {
                        product.UpdatePrice(eventData.Price);

                        await repository.UpdateAsync(product);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

            }

        }
    }
}

