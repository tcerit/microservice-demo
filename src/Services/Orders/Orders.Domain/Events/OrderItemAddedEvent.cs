using System;
using Core.Events;

namespace Orders.Domain.Events
{
    public class OrderItemAddedEvent : DomainEvent
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public double UnitPrice { get; private set; }
        public int Quantity { get; private set; }

        public OrderItemAddedEvent(Guid orderId, Guid productId, string productName, double unitPrice, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}

