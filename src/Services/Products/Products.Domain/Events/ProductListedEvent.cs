using System;
using Core.Events;

namespace Products.Domain.Events
{
	public class ProductListedEvent : DomainEvent
	{
        public Guid ProductId { get; internal set; }

        public ProductListedEvent(Guid productId)
        {
            ProductId = productId;
        }
    }
}

