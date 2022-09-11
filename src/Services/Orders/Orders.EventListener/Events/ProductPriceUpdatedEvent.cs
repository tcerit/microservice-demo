using System;
using Core.Events;

namespace Orders.EventListener.Events
{
    public class ProductPriceUpdatedEvent : DomainEvent
    {
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
    }
}

