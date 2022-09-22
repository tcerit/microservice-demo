using System.Text;
using Ardalis.GuardClauses;
using Core.Events;
using Core.Settings;
using Microsoft.Extensions.Options;
using Orders.EventListener.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Orders.EventListener;

public class OrdersEventListenerWorker : BackgroundService
{
    private readonly ILogger<OrdersEventListenerWorker> _logger;
    private readonly MessageBrokerSettings _messageBroker;
    private readonly IDomainEventDispatcher _dispatcher;
    private IConnection? _connection;
    private IModel? _channel;
    private List<string> _queueNames = new List<string>();
    private const string CUSTOMER_EVENTS = "CustomerEvents";
    private const string PRODUCT_EVENTS = "ProductEvents";

    public OrdersEventListenerWorker(
        ILogger<OrdersEventListenerWorker> logger,
        IOptions<MessageBrokerSettings> messageBrokerOptions,
        IDomainEventDispatcher dispatcher)
    {
        _logger = logger;
        _messageBroker = messageBrokerOptions.Value;
        InitRabbitMQ();
        _dispatcher = dispatcher;
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
        Guard.Against.Null(_channel, nameof(IConnection));

        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        string queueName = _channel.QueueDeclare().QueueName;

        for (int i = 0; i<routingKeys.Length; i++)
        {
            _channel.QueueBind(queueName, exchangeName, routingKeys[i], null);
        }

        _queueNames.Add(queueName);
    }

    private async Task HandleMessage(string eventName, string message)
    {

        switch (eventName)
        {
            case nameof(CustomerCreatedEvent):
                await Dispatch<CustomerCreatedEvent>(message);
                break;
            case nameof(ProductCreatedEvent):
                await Dispatch<ProductCreatedEvent>(message);
                break;
            case nameof(ProductListedEvent):
                await Dispatch<ProductListedEvent>(message);
                break;
            case nameof(ProductDelistedEvent):
                await Dispatch<ProductDelistedEvent>(message);
                break;
        }

    }

    private async Task Dispatch<T>(string message) where T : IDomainEvent
    {
        var eventData = DomainEvent.Deserialize<T>(message);
        if (eventData != null)
            await _dispatcher.Dispatch(eventData);

    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        Guard.Against.Null(_channel, nameof(IConnection));


        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (ch, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine("{0} {1} {2}", DateTime.Now, ea.RoutingKey, message);

            await HandleMessage(ea.RoutingKey, message);
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _queueNames.ForEach(queueName => _channel.BasicConsume(queueName, false, consumer));

        return Task.CompletedTask;
    }
}
