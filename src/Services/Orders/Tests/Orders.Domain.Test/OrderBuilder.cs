using System;
namespace Orders.Domain.Test
{
    public class OrderBuilder
    {

        private Order _order;

        
        public int TestQuantity { get; private set; }

        public OrderBuilder()
        {
            
        }

        public Order BuildWithOrderStart()
        {
            Buyer buyer = Buyer.FromCustomer(Guid.NewGuid());
            _order = Order.Start(buyer);
            return _order;
        }

        public Order BuildWithItem(OrderProduct product, int quantity)
        {
            _order = BuildWithOrderStart();
            _order.AddItem(product, quantity);

            return _order;
        }
    }
}

