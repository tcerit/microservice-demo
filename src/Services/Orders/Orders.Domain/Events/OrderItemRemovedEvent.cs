using System;
using Core.Events;

namespace Orders.Domain.Events
{
    public class OrderItemRemovedEvent : DomainEvent
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }

        public OrderItemRemovedEvent(Guid orderId, Guid productId)
        {
            OrderId = orderId;
            ProductId = productId;
        }
    }
}

