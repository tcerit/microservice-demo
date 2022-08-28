using System;
using Core.Data;
using Orders.Domain;

namespace Orders.Application.Repositories
{
	public interface IOrderRepository : IRepository<Order>
	{
		Task<Order?> GetOrder(Guid id);
        Task<Order?> GetOrderWithItems(Guid id);
        Task<List<Order>> PlacedOrdersWithItems(DateTime startTime, DateTime endTime);
    }
}

