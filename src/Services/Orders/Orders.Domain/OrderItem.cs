using System;
using Ardalis.GuardClauses;
using Core.Domain;
using Core.Guards;

namespace Orders.Domain
{
    public class OrderItem : ValueObject
    {
        public OrderProduct Product { get; private set; }
        public int Quantity { get; private set; }

        private OrderItem()
        {
        }

        internal OrderItem(OrderProduct product, int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Product = product;
            Quantity = quantity;
        }

        public void Add(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Quantity += quantity;
        }

        public void SetNewQuantity(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Quantity = quantity;
        }
        
    }
}

