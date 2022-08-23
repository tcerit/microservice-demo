using System;
using Core.Domain;

namespace Orders.Domain
{
    public class OrderStatus : Enumeration
    {

        public static readonly OrderStatus UNSET = new OrderStatus(0, "Unset");
        public static readonly OrderStatus DRAFT = new OrderStatus(1, "Draft");
        public static readonly OrderStatus PLACED = new OrderStatus(2, "Placed");
        public static readonly OrderStatus CANCELLED = new OrderStatus(3, "Cancelled");

        public OrderStatus()
        {
        }

        private OrderStatus(int value, string displayName) : base(value, displayName)
        {
        }

        




    }


    
}

