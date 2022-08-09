using System;
namespace Core.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime DateOccured { get; set; } = DateTime.Now;
    }
}

