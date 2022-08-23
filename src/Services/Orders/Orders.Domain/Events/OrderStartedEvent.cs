using System;
using Core.Events;

namespace Orders.Domain.Events
{
    public class OrderStartedEvent : DomainEvent
    {
        public Guid OrderId { get; private set; }
        public Guid BuyerId { get; private set; }

        public OrderStartedEvent(Guid orderId, Guid buyerId)
        {
            OrderId = orderId;
            
        }
    }
}

