using System.Text;
using System.Threading.Channels;
using Core.Data;
using Core.Data.Outbox;
using Core.Events;
using Core.Messaging;
using Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Orders.MessageRelay;

public class OrdersMessageRelayWorker : BackgroundService
{
    private readonly ILogger<OrdersMessageRelayWorker> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly IOutboxManager _outboxManager;

    public OrdersMessageRelayWorker(
        ILogger<OrdersMessageRelayWorker> logger,
        IMessageBroker messageBroker,
        IOutboxManager outboxManager)
    {
        _logger = logger;

        _messageBroker = messageBroker;
        _messageBroker.Init();

        _outboxManager = outboxManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("{0} - Checking Outbox", DateTime.Now);

            await _outboxManager.SendPendingItemsAsync();

            await Task.Delay(5000, stoppingToken);
        }
    }
}

