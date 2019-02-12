using olmelabs.battleship.api.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services.Interfaces
{
    public interface IGameStatisticsService
    {
        void EnqueueGameStatistics(List<ShipInfo> clientShips);

        List<int> TryDequeueGameStatistics();

        Task<ClientStatistics> GetStatisticsAsync();

        Task UpdateStatisticsAsync(Dictionary<int, int> cellUsage);
    }
}
