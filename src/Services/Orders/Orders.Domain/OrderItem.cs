using System;
using Ardalis.GuardClauses;
using Core.Domain;
using Core.Guards;

namespace Orders.Domain
{
    public class OrderItem : ValueObject
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public double UnitPrice { get; private set; }
        public int Quantity { get; private set; }

        private OrderItem()
        {
        }

        internal OrderItem(Guid productId, string productName, double unitPrice, int quantity)
        {
            Guard.Against.EmptyGuid(productId, nameof(productId));
            Guard.Against.NullOrWhiteSpace(productName, nameof(productName));
            Guard.Against.Negative(unitPrice, nameof(unitPrice));
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public void Add(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Quantity += quantity;
        }

        public void Remove(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Quantity -= quantity;
        }

        public void SetNewQuantity(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Quantity -= quantity;
        }
        
    }
}

