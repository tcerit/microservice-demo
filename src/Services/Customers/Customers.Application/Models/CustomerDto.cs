using System;
namespace Customers.Application.Models
{
	public class CustomerDto
	{
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public decimal LoyaltyPoints { get; set; }
    }
}

