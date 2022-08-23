using System;
using Core.Events;

namespace Customers.EventListener.Events
{
	public class OrderPlacedEvent : DomainEvent
	{
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public decimal OrderTotal { get; set; }
    }
}

