using System;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using Orders.Domain;

namespace Orders.Application.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(context)
        {
        }

        public Task<Order?> GetOrder(Guid id)
        {
            return FindById(id).Include(p => p.Buyer).SingleOrDefaultAsync();
        }

        public Task<Order?> GetOrderWithItems(Guid id)
        {
            return FindById(id).Include(p => p.Buyer).SingleOrDefaultAsync();
        }
    }
}

