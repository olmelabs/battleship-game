using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using olmelabs.battleship.api.Helpers;
using olmelabs.battleship.api.Models;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services.Interfaces;
using olmelabs.battleship.api.SignalRHubs;

namespace olmelabs.battleship.api.BackgroundServices
{
    /// <summary>
    /// Background worker using IHostedServide
    /// https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/multi-container-microservice-net-applications/background-tasks-with-ihostedservice
    /// </summary>
    public class PlayerService : BackgroundService
    {
        private readonly ILogger<PlayerService> _logger;
        private readonly GameOptions _options;
        private readonly IHubContext<GameHub> _gameHubContext;
        private readonly IGameService _gameSvc;

        public PlayerService(IGameService gameSvc,
            IHubContext<GameHub> gameHubContext,
            IOptions<GameOptions> optionsAccessor,
            ILogger<PlayerService> logger)
        {
            _logger = logger;
            _options = optionsAccessor.Value;
            _gameSvc = gameSvc;
            _gameHubContext = gameHubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"PlayerService is starting.");

            //something to be called on cancellation
            stoppingToken.Register(() => _logger.LogDebug($"PlayerService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                var activeGames = _gameSvc.FindAllActiveGames().Where(g => g.IsAwaitingServerTurn);

                activeGames.ForEach(async game =>
                {
                    try
                    {
                        await MakeMove(game);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"PlayerService.MakeMove exception. Game: {game.GameId}");
                    }
                });
                

                //my method here
                await Task.Delay(_options.BackgroundPlayerDelayTime, stoppingToken);
            }
        }

        private async Task MakeMove(GameState game)
        {
            int cellId = await _gameSvc.FiredCannonFromServer(game.GameId);
            if (cellId < 0)
                return;

            FireCannonDto dto = new FireCannonDto
            {
                GameId = game.GameId,
                CellId = cellId
            };

            _logger.LogDebug($"BackgroundPlayerService: Sent to Connection id: {game.ConnectionId}. Game id: {dto.GameId}. Cell id {dto.CellId}.");

            await _gameHubContext.Clients.Client(game.ConnectionId).SendAsync("MakeFireFromServer", dto);
        }
    }
}
