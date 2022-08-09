using System;
using Core.Events;

namespace Orders.Domain.Events
{
    public class OrderStartedEvent : DomainEvent
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }

        public OrderStartedEvent(Guid orderId, Guid customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }
    }
}

