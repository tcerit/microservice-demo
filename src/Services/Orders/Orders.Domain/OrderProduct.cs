using System;
using Core.Domain;

namespace Orders.Domain
{
	public class OrderProduct : Entity
	{

        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public bool IsAvailable { get; private set; }

        private OrderProduct()
        {
        }

        private OrderProduct(Guid id, string name, decimal price) : base(id)
        {
            Name = name;
            Price = price;
            IsAvailable = false;
        }

        public static OrderProduct FromProduct(Guid productId, string productName, decimal unitPrice)
        {
            OrderProduct product = new(productId, productName, unitPrice);
            return product;
        }

        public void MakeAvailable()
        {
            IsAvailable = true;
        }

        public void MakeUnavailable()
        {
            IsAvailable = false;
        }
    }
}

