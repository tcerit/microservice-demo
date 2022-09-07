using System;
namespace Products.Application.Models
{
	public class ProductDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Price { get; set; }
		public string PictureUrl{ get; set; }
	}
}

