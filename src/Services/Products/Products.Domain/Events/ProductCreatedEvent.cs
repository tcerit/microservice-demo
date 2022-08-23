using System;
using Core.Events;

namespace Products.Domain.Events
{
	public class ProductCreatedEvent : DomainEvent
	{
		public Guid ProductId { get; private set; }
		public string Name { get; private set; }
		public decimal Price { get; private set; }

        public ProductCreatedEvent(Guid productId, string name, decimal price)
        {
            ProductId = productId;
            Name = name;
            Price = price;
        }
    }
}

