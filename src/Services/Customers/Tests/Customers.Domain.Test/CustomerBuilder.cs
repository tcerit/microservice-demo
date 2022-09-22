using System;
namespace Customers.Domain.Test
{
	public class CustomerBuilder
	{
        private Customer _customer;

        public decimal TestLoyaltyPoints { get; private set; } = 100;

        public CustomerBuilder()
        {
        }

        public Customer BuildDefault()
        {
            _customer = Customer.Create("name", "lastName");
            return _customer;
        }

        public Customer BuildHavingSomeLoyaltyPoints()
        {
            BuildDefault();

            _customer.EarnPointsFromOrder(TestLoyaltyPoints);
   
            return _customer;
        }

    }
}

