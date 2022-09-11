

using System;
using System.Runtime;
using Ardalis.GuardClauses;
using Core.Data.Outbox;
using Core.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Core.Messaging
{
	public class MessageBroker : IMessageBroker
	{
        private IConnection? _connection;
        private IModel? _channel;
        private IBasicProperties? _messageProperties;
        private readonly MessageBrokerSettings _messageBroker;

        public MessageBroker(IOptions<MessageBrokerSettings> messageBrokerOptions)
		{
            Guard.Against.Null(messageBrokerOptions, nameof(messageBrokerOptions));
            _messageBroker = messageBrokerOptions.Value;
        }

        public void Init()
        {
            Guard.Against.NullOrWhiteSpace(_messageBroker.Uri, nameof(ConnectionFactory.Uri));

            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri(_messageBroker.Uri);
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_messageBroker.ExchangeName, ExchangeType.Direct);

            _messageProperties = _channel.CreateBasicProperties();
            _messageProperties.ContentType = _messageBroker.ContentType;
        }

        public void Publish(byte[] message, string messageType)
        {
            Guard.Against.Null(_channel, nameof(IConnection));

            _channel.BasicPublish(exchange: _messageBroker.ExchangeName,
                                     routingKey: messageType,
                                     basicProperties: _messageProperties,
                                     body: message);
        }
    }
}

