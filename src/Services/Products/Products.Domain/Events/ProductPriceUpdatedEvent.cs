using System;
namespace Products.Domain.Events
{
	public class ProductPriceUpdatedEvent
	{
        public Guid ProductId { get; private set; }
        public decimal Price { get; private set; }

        public ProductPriceUpdatedEvent(Guid productId, decimal price)
        {
            ProductId = productId;
            Price = price;
        }
    }
}

