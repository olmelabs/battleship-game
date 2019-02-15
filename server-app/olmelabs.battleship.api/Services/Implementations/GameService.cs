using Microsoft.Extensions.Options;
using olmelabs.battleship.api.Logic;
using olmelabs.battleship.api.Models;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;
using olmelabs.battleship.api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services.Implementations
{
    public class GameService : IGameService
    {
        private readonly IStorage _storage;
        private readonly IGameLogic _gameLogic;
        private readonly IGameStatisticsService _gameStatistics;
        private readonly GameOptions _options;

        public GameService(IStorage storage, IGameLogic gameLogic, 
            IGameStatisticsService gameStatistics,
            IOptions<GameOptions> optionsAccessor)
        {
            _storage = storage;
            _gameLogic = gameLogic;
            _gameStatistics = gameStatistics;
            _options = optionsAccessor.Value;
        }

        public virtual async Task<GameState> StartNewGameAsync(string connectionId)
        {
            GameState g = GameState.CreateNew(_gameLogic);
            g.ConnectionId = connectionId;

            return await _storage.AddGameAsync(g);
        }

        public virtual async Task<GameState> StopGameAsync(string gameId, List<ShipInfo> clientShips)
        {
            GameState g = await FindActiveGameAsync(gameId);
            if (g == null)
                return null;

            g.ClientBoard.Ships = clientShips;
            g.DateEnd = DateTime.Now;

            return await _storage.UpdateGameAsync(g);
        }

        public virtual async Task<GameState> FindActiveGameAsync(string gameId)
        {
            GameState g = await _storage.FindActiveGameAsync(gameId);
            return g;
        }

        public virtual IQueryable<GameState> FindAllActiveGames()
        {
            var res = _storage.SelectGames(x => x.DateEnd == null);
            return res;
        }

        public virtual async Task<FireResult> FireCannon(string gameId, int cellId)
        {
            GameState g = await FindActiveGameAsync(gameId);
            if (g == null)
                return null;

            FireResult ret = new FireResult
            {
                IsHit = g.ServerBoard.Board[cellId] == (int)ServerCellState.CellHasShip
            };

            if (ret.IsHit)
            {
                var ship = g.ServerBoard.Ships.First(x => x.Cells.Any(c => c == cellId));
                ship.Hits++;
                if (ship.Hits == ship.Cells.Count())
                {
                    ret.ShipDestroyed = ship;
                }

                ret.IsGameOver = !g.ServerBoard.Ships.Any(x => x.Hits < x.Cells.Count());
            }
            else
            {
                g.IsAwaitingServerTurn = true;
            }

            await _storage.UpdateGameAsync(g);

            return ret;
        }

        public virtual async Task<int> FiredCannonFromServer(string gameId)
        {
            GameState g = await FindActiveGameAsync(gameId);
            if (g == null)
                return -1;

            ClientStatistics stat = null;
            if (_options.UseStatistics)
            {
                stat = await _gameStatistics.GetStatisticsAsync();
            }

            int i = _gameLogic.ChooseNextClientCell(g.ClientBoard, g.CurrentShip, stat);

            g.ClientBoard.Board[i] = (int)ClientCellState.CellFiredResultNotYetKnown;

            g.IsAwaitingServerTurn = false;

            await _storage.UpdateGameAsync(g);

            return i;
        }

        public virtual async Task<GameState> FireCannonProcessResult(FireCannonCallbackDto fireResult)
        {
            GameState g = await FindActiveGameAsync(fireResult.GameId);

            g.ClientBoard.Board[fireResult.CellId] = fireResult.Result ? (int)ClientCellState.CellFiredAndShipHit : (int)ClientCellState.CellFiredButEmpty;
            g.IsAwaitingServerTurn = fireResult.Result;

            //if we ship is hit but not destoyed - try to make fires around it on next turn
            if (fireResult.ShipDestroyed == null && fireResult.Result)
            {
                g.CurrentShip.Add(fireResult.CellId);
            }

            //record destroyed ship
            if (fireResult.ShipDestroyed != null)
            {
                bool isVertical = fireResult.ShipDestroyed.Length > 1 && fireResult.ShipDestroyed[1] - fireResult.ShipDestroyed[0] == 10;
                ShipInfo ship = new ShipInfo(isVertical, fireResult.ShipDestroyed);
                g.ClientBoard.Ships.Add(ship);
                _gameLogic.MarkCellsAroundShip(g.ClientBoard, ship);
                g.CurrentShip.Clear();
            }

            g = await _storage.UpdateGameAsync(g);
            return g;
        }

        public bool ValidateClientBoard(List<ClientShipDto> ships)
        {
            return _gameLogic.ValidateClientBoard(ships);
        }

        public BoardInfo GenerateClientBoard()
        {
            return _gameLogic.GenerateBoad();
        }
    }
}
