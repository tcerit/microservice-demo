using System;
namespace Orders.Domain.Test
{
	public class BuyerBuilder
	{
		private Buyer _buyer;

		public BuyerBuilder()
		{
			
		}

		public Buyer BuildDefault()
		{
			_buyer = Buyer.FromCustomer(Guid.NewGuid());
			return _buyer;
        }
	}
}

