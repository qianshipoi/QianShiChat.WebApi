﻿namespace QianShiChat.WebApi.BackgroundHost;

public sealed class ExpiredFilesCleanupService : IHostedService, IDisposable
{
    private readonly ITusExpirationStore _expirationStore;
    private readonly ExpirationBase _expiration;
    private readonly ILogger<ExpiredFilesCleanupService> _logger;
    private Timer? _timer;

    public ExpiredFilesCleanupService(ILogger<ExpiredFilesCleanupService> logger, DefaultTusConfiguration config)
    {
        _logger = logger;
        _expirationStore = (ITusExpirationStore)config.Store;
        _expiration = config.Expiration;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_expiration == null)
        {
            _logger.LogInformation("Not running cleanup job as no expiration has been set.");
            return;
        }

        await RunCleanup(cancellationToken);
        _timer = new Timer(async (e) => await RunCleanup((CancellationToken)e!), cancellationToken, TimeSpan.Zero, _expiration.Timeout);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async Task RunCleanup(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Running cleanup job...");
            var numberOfRemovedFiles = await _expirationStore.RemoveExpiredFilesAsync(cancellationToken);
            _logger.LogInformation($"Removed {numberOfRemovedFiles} expired files. Scheduled to run again in {_expiration.Timeout.TotalMilliseconds} ms");
        }
        catch (Exception exc)
        {
            _logger.LogWarning("Failed to run cleanup job: " + exc.Message);
        }
    }
}
