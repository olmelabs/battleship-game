using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using olmelabs.battleship.api.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.BackgroundServices
{
    public class CleanupService : BackgroundService
    {
        private readonly IStorage _storage;
        private readonly ILogger<CleanupService> _logger;

        public CleanupService(ILogger<CleanupService> logger, IStorage storage)
        {
            _storage = storage;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"CleanupService is starting.");

                stoppingToken.Register(() =>
                {
                    _logger.LogDebug($"CleanupService is stopping.");
                });

                await _storage.Cleanup();

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
