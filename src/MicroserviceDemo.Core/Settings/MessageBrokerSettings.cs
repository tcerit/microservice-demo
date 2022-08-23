using System;
namespace Core.Settings
{
    public class MessageBrokerSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
    }
}

