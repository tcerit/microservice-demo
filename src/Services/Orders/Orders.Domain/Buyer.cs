using System;
using Core.Domain;

namespace Orders.Domain
{
    public class Buyer : Entity
    {
        public string FullName { get; private set; }
        private readonly List<Order> _orders = new();
        public IReadOnlyCollection<Order> Orders => _orders;

        private Buyer()
        {
        }

        private Buyer(Guid id, string fullName) : base(id)
        {
            FullName = fullName;
        }

        public static Buyer FromCustomer(Guid customerId, string fullName) => new(customerId, fullName);

        public Order StartOrdering()
        {
            return Order.Start(this);
        }
    }
}

