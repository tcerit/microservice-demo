using System;
using Core.Events;

namespace Orders.Domain.Events
{
    public class OrderPlacedEvent : DomainEvent
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public double OrderTotal { get; private set; }

        public OrderPlacedEvent(Guid orderId, Guid customerId, double orderTotal)
        {
            OrderId = orderId;
            CustomerId = customerId;
            OrderTotal = orderTotal;
        }
    }
}

