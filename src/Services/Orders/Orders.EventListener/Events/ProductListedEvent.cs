using System;
using Core.Events;

namespace Orders.EventListener.Events
{
	public class ProductListedEvent : DomainEvent
    {
        public Guid ProductId { get; set; }
    }
}


