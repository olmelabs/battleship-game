
using olmelabs.battleship.api.Models.Entities;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services.Interfaces
{
    public interface IPeerToPeerGameService
    {
        Task<PeerToPeerGameState> StartNewSessionAsync(string hostConnectionId);

        Task<PeerToPeerGameState> JoinSessionAsync(string code, string connectionId);

        Task<PeerToPeerGameState> AddPeerToGame(string code, string connectionId);
    }
}
