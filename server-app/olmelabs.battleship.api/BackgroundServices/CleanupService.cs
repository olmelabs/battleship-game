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
            _logger.LogDebug($"CleanupService is starting.");

            stoppingToken.Register(() =>
            {
                _logger.LogDebug($"CleanupService is stopping.");
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                await _storage.Cleanup();

                //sleep 20 minutes
                await Task.Delay(1000 * 60 * 20, stoppingToken);
            }
        }
    }
}
