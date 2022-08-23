using System;
using Core.Events;

namespace Orders.EventListener.Events
{
	public class ProductCreatedEvent : DomainEvent
	{
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

