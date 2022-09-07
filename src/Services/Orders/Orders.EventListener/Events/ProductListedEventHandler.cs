using System;
using Core.Configuration;
using Core.Data;
using Core.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Orders.Data;
using Orders.Domain;

namespace Orders.EventListener.Events
{
    public class ProductListedEventHandler : INotificationHandler<DomainEventNotification<ProductListedEvent>>
    {
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly IRepositoryScopeFactory<OrdersDataContext> _serviceScopeFactory;

        public ProductListedEventHandler(
            IRepositoryScopeFactory<OrdersDataContext> serviceScopeFactory,
            ILogger<ProductCreatedEventHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
      

        public async Task Handle(DomainEventNotification<ProductListedEvent> notification, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var _repository = scope.GetRepository<OrderProduct>();

                    ProductListedEvent eventData = notification.DomainEvent;

                    OrderProduct? product = await _repository.GetByIdAsync(eventData.ProductId);
                    if (product != null)
                    {
                        product.MakeAvailable();
                        await _repository.UpdateAsync(product);
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            
        }
    }
}

