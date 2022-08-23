using System;
using Core.Events;

namespace Customers.Domain.Events
{
    public class CustomerCreatedEvent: DomainEvent
    {
        public Guid CustomerId { get; private set; }

        public CustomerCreatedEvent(Guid id)
        {
            CustomerId = id;
        }
    }
}

