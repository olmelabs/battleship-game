using System.Collections.Generic;
using olmelabs.battleship.api.Models.Entities;
using System.Linq;
using System.Collections.Concurrent;
using olmelabs.battleship.api.Repositories;
using System.Threading.Tasks;
using olmelabs.battleship.api.Helpers;

namespace olmelabs.battleship.api.Services
{
    public class GameStatisticsService : IGameStatisticsService
    {
        private static ConcurrentQueue<List<int>> _queue;
        private readonly IStorage _storage;

        static GameStatisticsService()
        {
            _queue = new ConcurrentQueue<List<int>>();
        }

        public GameStatisticsService(IStorage storage, IGameLogic gameLogic)
        {
            _storage = storage;
        }

        public void EnqueueGameStatistics(List<ShipInfo> clientShips)
        {
            var celsWithShip = clientShips.SelectMany(s => s.Cells).ToList();
            _queue.Enqueue(celsWithShip);
        }

        public List<int> TryDequeueGameStatistics()
        {
            bool res = _queue.TryDequeue(out List<int> cells);
            if (res)
                return cells;
            return null;
        }
        public async Task<ClientStatistics> GetStatisticsAsync()
        {
            return await _storage.GetClientStatisticsAsync();
        }

        public async Task UpdateStatisticsAsync(Dictionary<int, int> cellUsage)
        {
            ClientStatistics stat = await _storage.GetClientStatisticsAsync();

            cellUsage.Keys.ForEach(key =>
            {
                stat.CellHits[key] += cellUsage[key];
            });

            await _storage.UpdateClientStatisticsAsync(stat);
        }
    }
}
