
using olmelabs.battleship.api.Models.Entities;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services
{
    public interface IPeerToPeerGameService
    {
        Task<PeerToPeerGameState> StartNewSessionAsync(string hostConnectionId);

        Task<PeerToPeerGameState> JoinSessionAsync(string code, string connectionId);
    }
}
