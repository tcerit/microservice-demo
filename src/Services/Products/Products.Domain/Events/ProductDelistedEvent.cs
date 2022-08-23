using System;
using Core.Events;

namespace Products.Domain.Events
{
	public class ProductDelistedEvent : DomainEvent
	{
		public Guid ProductId { get; internal set; }

        public ProductDelistedEvent(Guid productId)
        {
            ProductId = productId;
        }
    }
}

