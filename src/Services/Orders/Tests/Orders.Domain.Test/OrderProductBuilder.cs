using System;
namespace Orders.Domain.Test
{
	public class OrderProductBuilder
	{
        private OrderProduct _orderProduct;

        public string TestProductName => "TestProduct";
        public decimal TestUnitPrice => 72m;

        public string TestDiffirentProductName => "DifferentTestProduct";
        public decimal TestDifferentProductUnitPrice => 34m;

        public OrderProductBuilder()
        {
        }

        public OrderProduct BuildDefault()
        {
            _orderProduct = OrderProduct.FromProduct(Guid.NewGuid(), TestProductName, TestUnitPrice);
            return _orderProduct;
        }

        public OrderProduct BuildDifferent()
        {
            _orderProduct = OrderProduct.FromProduct(Guid.NewGuid(), TestDiffirentProductName, TestDifferentProductUnitPrice);
            return _orderProduct;
        }
    }
}

