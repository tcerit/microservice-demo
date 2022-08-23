using System;
using Core.Events;

namespace Orders.EventListener.Events
{
	public class ProductDelistedEvent : DomainEvent
    {
        public Guid ProductId { get; set; }
    }
}

