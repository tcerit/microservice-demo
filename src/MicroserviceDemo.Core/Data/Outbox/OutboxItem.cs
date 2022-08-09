using System;
using System.Text.Json;
using Core.Domain;
using Core.Events;

namespace Core.Data.Outbox
{
    public class OutboxItem : Entity
    {
        public DateTime DateOccured { get; private set; }
        public string Data { get; private set; }
        public string Type { get; private set; }

        private OutboxItem() { }

        private OutboxItem(Guid id, string type, string data, DateTime dateOccured) : base(id)
        {
            Type = type;
            Data = data;
            DateOccured = dateOccured;
        }

        public static OutboxItem FromEvent(IDomainEvent domainEvent) => new(Guid.NewGuid(), domainEvent.GetType().Name, JsonSerializer.Serialize(domainEvent), domainEvent.DateOccured);


    }
}

