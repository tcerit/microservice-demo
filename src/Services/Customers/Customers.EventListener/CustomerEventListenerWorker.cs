using System.Text;
using Core.Events;
using Core.Settings;
using Customers.EventListener.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Customers.EventListener;

public class CustomerEventListenerWorker : BackgroundService
{
    private readonly ILogger<CustomerEventListenerWorker> _logger;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly MessageBrokerSettings _messageBroker;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;
    private const string ORDER_EVENTS = "OrderEvents";

    public CustomerEventListenerWorker(ILogger<CustomerEventListenerWorker> logger, IOptions<MessageBrokerSettings> messageBrokerOptions, IDomainEventDispatcher dispatcher)
    {
        _logger = logger;
        _messageBroker = messageBrokerOptions.Value;
        _dispatcher = dispatcher;
        InitRabbitMQ();
    }

    private void InitRabbitMQ()
    {
        Console.WriteLine(_messageBroker.Uri);
        ConnectionFactory factory = new ConnectionFactory();
        factory.Uri = new Uri(_messageBroker.Uri);
        _connection = factory.CreateConnection(); 
        _channel = _connection.CreateModel();

        Subscribe(ORDER_EVENTS, new string[] { nameof(OrderPlacedEvent) });

    }

    private void Subscribe(string exchangeName, string[] routingKeys)
    {
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        if (_queueName == null)
        {
            _queueName = _channel.QueueDeclare().QueueName;
        }
        else
        {
            _channel.QueueDeclare(_queueName);
        }

        for (int i = 0; i < routingKeys.Length; i++)
        {
            _channel.QueueBind(_queueName, exchangeName, routingKeys[i], null);
        }

        _channel.BasicQos(0, 1, false);
    }

    private void HandleMessage(string eventName, string message)
    {

        switch (eventName)
        {
            case nameof(OrderPlacedEvent):
                Dispatch<OrderPlacedEvent>(message);
                break;
        }

    }

    private void Dispatch<T>(string message) where T : IDomainEvent
    {
        var eventData = DomainEvent.Deserialize<T>(message);
        if (eventData != null)
            _dispatcher.Dispatch(eventData);
    }




    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine("{0} {1} {2}", DateTime.Now, ea.RoutingKey, message);
            HandleMessage(ea.RoutingKey, message);
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(_queueName, false, consumer);

        return Task.CompletedTask;


    }
}

