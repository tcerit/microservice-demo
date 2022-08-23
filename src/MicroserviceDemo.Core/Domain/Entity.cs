using System;
using System.Collections.Concurrent;
using Core.Events;

namespace Core.Domain
{
    public abstract class Entity { 

        public Guid Id { get; private set; }
        

        protected Entity() { }

        protected Entity(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException("The ID cannot be the default value.", nameof(id));
            }

            Id = id;
        }

        private ConcurrentQueue<IDomainEvent> _domainEvents = new ConcurrentQueue<IDomainEvent>();
        public IProducerConsumerCollection<IDomainEvent> DomainEvents => _domainEvents;

        protected void AddDomainEvent(IDomainEvent @eventItem)
        {
            _domainEvents.Enqueue(@eventItem);
        }

        protected void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

       
    }
}

