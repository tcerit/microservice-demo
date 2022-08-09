using System;
using Core.Events;

namespace Orders.Domain.Events
{
    public class OrderItemChangedEvent : DomainEvent
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public OrderItemChangedEvent(Guid orderId, Guid productId, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}

