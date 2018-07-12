
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services
{
    public interface IGameService
    {
        Task<GameState> StartNewGameAsync(string connectionId);

        Task<GameState> StopGameAsync(string gameId, List<ShipInfo> clientShips);

        Task<FireResult> FireCannon(string gameId, int cellId);

        Task<GameState> FireCannonProcessResult(FireCannonResponseDto fireResult);

        Task<int> FiredCannonFromServer(string gameId);

        Task<GameState> FindActiveGameAsync(string gameId);
        
        IQueryable<GameState> FindAllActiveGames();

        bool ValidateClientBoard(List<ClientShipDto> ships);

        BoardInfo GenerateClientBoard();
    }
}
