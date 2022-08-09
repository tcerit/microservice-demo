using System;
using MediatR;

namespace Core.Events
{
    public class DomainEventNotification<T> : INotification where T : IDomainEvent
    {
        public T DomainEvent { get; }

        public DomainEventNotification(T domainEvent)
        {
            DomainEvent = domainEvent;
        }

    }
}

