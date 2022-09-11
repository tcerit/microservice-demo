using System;
namespace Core.Settings
{
    public class MessageBrokerSettings
    {
        public string? Uri { get; set; }
        public string? ExchangeName { get; set; }
        public string ContentType { get; set; } = "text/json";
    }
}

