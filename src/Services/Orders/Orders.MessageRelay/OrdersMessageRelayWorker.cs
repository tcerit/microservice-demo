using System.Text;
using System.Threading.Channels;
using Core.Data;
using Core.Data.Outbox;
using Core.Events;
using Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Orders.MessageRelay;

public class OrdersMessageRelayWorker : BackgroundService
{
    private readonly ILogger<OrdersMessageRelayWorker> _logger;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private readonly IRepository<OutboxItem> _repository;
    private IConnection _connection;
    private IModel _channel;
    private string _exchangeName;
    private Dictionary<ulong, OutboxItem> unqueuedItems = new();
    private List<OutboxItem> _changeList = new();

    public OrdersMessageRelayWorker(
        ILogger<OrdersMessageRelayWorker> logger,
        IOptions<MessageBrokerSettings> messageBrokerOptions,
        IRepository<OutboxItem> repository)
    {
        _logger = logger;
        _messageBrokerSettings = messageBrokerOptions.Value;
        _exchangeName = "OrderEvents";
        InitRabbitMQ();
        _repository = repository;
    }

    private void InitRabbitMQ()
    {
        ConnectionFactory factory = new ConnectionFactory();
        factory.UserName = _messageBrokerSettings.Username;
        factory.Password = _messageBrokerSettings.Password;
        factory.VirtualHost = _messageBrokerSettings.VirtualHost;
        factory.HostName = _messageBrokerSettings.HostName;

        // create connection  
        _connection = factory.CreateConnection();

        // create channel  
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var messageProperties = _channel.CreateBasicProperties();
        messageProperties.ContentType = "text/json";

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("{0} - Checking Outbox", DateTime.Now);
            List<OutboxItem> unsentOutboxList = await _repository.Find(p => p.State == 0).ToListAsync();

            unsentOutboxList.ForEach(outboxItem =>
            {
                var body = Encoding.UTF8.GetBytes(outboxItem.Data);
                // unqueuedItems.Add(channel.NextPublishSeqNo, outboxItem);
                _channel.BasicPublish(exchange: _exchangeName,
                                     routingKey: outboxItem.Type,
                                     basicProperties: messageProperties,
                                     body: body);
                outboxItem.Send();
                _changeList.Add(outboxItem);
                _logger.LogInformation("{0} {1} {2}", DateTime.Now, outboxItem.Type, outboxItem.Data);
            });



            await Task.Delay(1000, stoppingToken);
            await _repository.UpdateRangeAsync(_changeList);
            _changeList.Clear();
        }
    }
}

