using System;
using Core.Events;

namespace Orders.Domain.Events
{
    public class OrderPlacedEvent : DomainEvent
    {
        public Guid OrderId { get; private set; }
        public Guid BuyerId { get; private set; }
        public decimal OrderTotal { get; private set; }

        public OrderPlacedEvent(Guid orderId, Guid buyerId, decimal orderTotal)
        {
            OrderId = orderId;
            BuyerId = buyerId;
            OrderTotal = orderTotal;
        }
    }
}

