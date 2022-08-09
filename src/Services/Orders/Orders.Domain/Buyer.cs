using System;
using Core.Domain;

namespace Orders.Domain
{
    public class Buyer : Entity
    {
        private readonly List<Order> _orders = new();
        public IReadOnlyCollection<Order> Orders => _orders;

        private Buyer()
        {
        }

        private Buyer(Guid id) : base(id)
        {
        }

        public static Buyer Create(Guid id) => new(id);

        public Order StartOrdering()
        {
            return Order.Start(this);
        }
    }
}

