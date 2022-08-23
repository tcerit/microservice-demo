using System.Text;
using Core.Data;
using Core.Data.Outbox;
using RabbitMQ.Client;

namespace Customers.MessageRelay;

public class CustomersMessageRelayWorker : BackgroundService
{
    private readonly ILogger<CustomersMessageRelayWorker> _logger;
    private readonly IRepository<OutboxItem> _repository;
    private Dictionary<ulong, OutboxItem> unqueuedItems = new();

    private List<OutboxItem> _changeList = new();
    public CustomersMessageRelayWorker(ILogger<CustomersMessageRelayWorker> logger, IRepository<OutboxItem> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ConnectionFactory factory = new ConnectionFactory();
        // "guest"/"guest" by default, limited to localhost connections
        factory.UserName = "guest";
        factory.Password = "guest";
        factory.VirtualHost = "/";
        factory.HostName = "127.0.0.1";

        _logger.LogInformation("Started");

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {

            string exchangeName = "CustomerEvents";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

            var messageProperties = channel.CreateBasicProperties();
            messageProperties.ContentType = "text/json";


            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("{0} - Checking Outbox", DateTime.Now);
                List<OutboxItem> unsentOutboxList = await _repository.FindAllAsync(p => p.State == 0);
                
                unsentOutboxList.ForEach(outboxItem =>
                {
                    var body = Encoding.UTF8.GetBytes(outboxItem.Data);
                   // unqueuedItems.Add(channel.NextPublishSeqNo, outboxItem);
                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: outboxItem.Type,
                                         basicProperties: messageProperties,
                                         body: body);
                    outboxItem.Send();
                    _changeList.Add(outboxItem);
                    _logger.LogInformation("{0} {1} {2}", DateTime.Now, outboxItem.Type, outboxItem.Data);
                });

                
                
                await Task.Delay(1000);
                await _repository.UpdateRangeAsync(_changeList);
                _changeList.Clear();

            }
        }
    }
}
