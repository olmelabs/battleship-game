using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using olmelabs.battleship.api.Models;
using olmelabs.battleship.api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.BackgroundServices
{
    public class StatisticsCollectorService : BackgroundService
    {
        private readonly ILogger<StatisticsCollectorService> _logger;
        private readonly GameOptions _options;
        private readonly IGameStatisticsService _statisticsService;

        public StatisticsCollectorService(
            IGameStatisticsService statisticsService,
            IOptions<GameOptions> optionsAccessor,
            ILogger<StatisticsCollectorService> logger)
        {
            _logger = logger;
            _options = optionsAccessor.Value;
            _statisticsService = statisticsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"StatisticsCollectorService is starting.");

            stoppingToken.Register(() => _logger.LogDebug($"StatisticsCollectorService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                //key - cellid. value - hit count
                Dictionary<int, int> cellUsage = new Dictionary<int, int>();
                List<int> gameCells = _statisticsService.TryDequeueGameStatistics();

                while (gameCells != null)
                {
                    gameCells.ForEach(cell => {
                        if (cellUsage.ContainsKey(cell))
                            cellUsage[cell]++;
                        else
                            cellUsage.Add(cell, 1);
                    });

                    gameCells = _statisticsService.TryDequeueGameStatistics();
                }

                if (cellUsage.Count > 0)
                {
                    _logger.LogDebug($"StatisticsCollectorService - statistics updated.");
                    await _statisticsService.UpdateStatisticsAsync(cellUsage);
                }

                await Task.Delay(_options.BackgroundStatisticsCollectorDelayTime, stoppingToken);
            }
        }
    }
}
