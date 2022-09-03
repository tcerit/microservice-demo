﻿using System.Text;
using System.Threading.Channels;
using Core.Data;
using Core.Data.Outbox;
using Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Products.MessageRelay;

public class ProductMessageRelayWorker : BackgroundService
{
    private readonly ILogger<ProductMessageRelayWorker> _logger;
    private readonly IRepository<OutboxItem> _repository;
    private readonly MessageBrokerSettings _messageBroker;
    private IConnection _connection;
    private IModel _channel;
    private List<OutboxItem> _changeList = new();
    private readonly string _exchangeName;

    public ProductMessageRelayWorker(ILogger<ProductMessageRelayWorker> logger, IOptions<MessageBrokerSettings> messageBrokerOptions, IRepository<OutboxItem> repository)
    {
        _logger = logger;
        _repository = repository;
        _messageBroker = messageBrokerOptions.Value;
        _exchangeName = "ProductEvents";
        InitRabbitMQ();
        
    }

    private void InitRabbitMQ()
    {
        ConnectionFactory factory = new ConnectionFactory();
        factory.Uri = new Uri(_messageBroker.Uri);
        _connection = factory.CreateConnection();
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
                _channel.BasicPublish(exchange: _exchangeName,
                                     routingKey: outboxItem.Type,
                                     basicProperties: messageProperties,
                                     body: body);
                outboxItem.Send();
                _changeList.Add(outboxItem);
                _logger.LogInformation("{0} {1} {2}", DateTime.Now, outboxItem.Type, outboxItem.Data);
            });



            await Task.Delay(5000, stoppingToken);
            await _repository.UpdateRangeAsync(_changeList);
            _changeList.Clear();
        }
    }
}

