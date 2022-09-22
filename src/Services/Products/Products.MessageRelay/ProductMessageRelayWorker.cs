using Core.Data.Outbox;
using Core.Messaging;

namespace Products.MessageRelay;

public class ProductMessageRelayWorker : BackgroundService
{
    private readonly ILogger<ProductMessageRelayWorker> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly IOutboxManager _outboxManager;

    public ProductMessageRelayWorker(
        ILogger<ProductMessageRelayWorker> logger,
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

