using System;
using Core.Events;

namespace Customers.Domain.Events
{
    public class CustomerCreatedEvent: DomainEvent
    {
        public Guid CustomerId { get; private set; }
        public string FullName { get; private set; }

        public CustomerCreatedEvent(Guid id, string fullName)
        {
            CustomerId = id;
            FullName = fullName;
        }
    }
}

