using System;
namespace Orders.Application.Models
{
	public class OrderDto
	{
		public string Buyer { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DatePlaced { get; set; }
		public List<OrderItemDto> Items { get; set; }
	}
}

