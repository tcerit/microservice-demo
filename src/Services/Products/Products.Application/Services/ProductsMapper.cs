using System;
using AutoMapper;
using Products.Application.Models;
using Products.Domain;

namespace Products.Application.Services
{
	public class ProductsMapper : Profile
	{
		public ProductsMapper()
		{
            CreateMap<Product, ProductDto>();
        }
	}
}

