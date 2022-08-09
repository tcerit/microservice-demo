using System;
using Core.Domain;

namespace Orders.Domain
{
    public class OrderStatus : Enumeration
    {

        public static readonly OrderStatus DRAFT = new OrderStatus(0, "Draft");
        public static readonly OrderStatus PLACED = new OrderStatus(1, "Placed");
        public static readonly OrderStatus CANCELLED = new OrderStatus(1, "Cancelled");

        public OrderStatus()
        {
        }

        private OrderStatus(int value, string displayName) : base(value, displayName)
        {
        }

        




    }


    
}

