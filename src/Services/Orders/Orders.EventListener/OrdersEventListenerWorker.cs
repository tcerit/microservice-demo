using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using Core.Events;
using Core.Settings;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Orders.EventListener.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Orders.EventListener;

public class OrdersEventListenerWorker : BackgroundService
{
    private readonly ILogger<OrdersEventListenerWorker> _logger;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly MessageBrokerSettings _messageBroker;
    private IConnection _connection;
    private IModel _channel;
    private List<string> _queueNames = new List<string>();
    private const string CUSTOMER_EVENTS = "CustomerEvents";
    private const string PRODUCT_EVENTS = "ProductEvents";

    public OrdersEventListenerWorker(ILogger<OrdersEventListenerWorker> logger, IOptions<MessageBrokerSettings> messageBrokerOptions, IDomainEventDispatcher dispatcher)
    {
        _logger = logger;
        _messageBroker = messageBrokerOptions.Value;
        _dispatcher = dispatcher;
        InitRabbitMQ();
    }

    private void InitRabbitMQ()
    {
        ConnectionFactory factory = new ConnectionFactory();
        factory.Uri = new Uri(_messageBroker.Uri);
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        Subscribe(CUSTOMER_EVENTS, new string[] { nameof(CustomerCreatedEvent) });
        Subscribe(PRODUCT_EVENTS, new string[] {
            nameof(ProductCreatedEvent),
            nameof(ProductListedEvent),
            nameof(ProductDelistedEvent)
        });
    }

    private void Subscribe(string exchangeName, string[] routingKeys)
    {
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        string queueName = _channel.QueueDeclare().QueueName;

        for (int i = 0; i<routingKeys.Length; i++)
        {
            _channel.QueueBind(queueName, exchangeName, routingKeys[i], null);
        }

        _queueNames.Add(queueName);
    }

    private void HandleMessage(string eventName, string message)
    {

        switch (eventName)
        {
            case nameof(CustomerCreatedEvent):
                Dispatch<CustomerCreatedEvent>(message);
                break;
            case nameof(ProductCreatedEvent):
                Dispatch<ProductCreatedEvent>(message);
                break;
            case nameof(ProductListedEvent):
                Dispatch<ProductListedEvent>(message);
                break;
            case nameof(ProductDelistedEvent):
                Dispatch<ProductDelistedEvent>(message);
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

        _queueNames.ForEach(queueName => _channel.BasicConsume(queueName, false, consumer));
        

        return Task.CompletedTask;

      
    }
}
