using System;
using Core.Events;

namespace Orders.EventListener.Events
{
    public class CustomerCreatedEvent : DomainEvent
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; }
    }
}

