using System;
using System.Text.Json;

namespace Core.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime DateOccured { get; set; } = DateTime.Now;

        public string Serialize()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                Converters = { new DomainEventConverter() },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public static T? Deserialize<T>(string json) where T : IDomainEvent
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}

