using System;
using AutoMapper;
using Orders.Application.Models;
using Orders.Domain;

namespace Orders.Application.Services
{
	public class OrdersMapper : Profile
	{
		public OrdersMapper()
		{
			CreateMap<Order, OrderDto>();
			CreateMap<OrderItem, OrderItemDto>();
		}
	}
}

