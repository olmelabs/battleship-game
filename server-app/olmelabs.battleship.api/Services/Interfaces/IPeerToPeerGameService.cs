
using olmelabs.battleship.api.Models.Entities;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services.Interfaces
{
    public interface IPeerToPeerGameService
    {
        Task<PeerToPeerSessionState> StartNewSessionAsync(string hostConnectionId);

        Task<PeerToPeerSessionState> JoinSessionAsync(string code, string connectionId);

        Task<PeerToPeerSessionState> AddPeerToSession(string code, string connectionId);

        Task<PeerToPeerSessionState> StartNewGameAsync(PeerToPeerSessionState session);

        Task<PeerToPeerSessionState> FindActiveSessionAsync(string code, string connectionId);
    }
}
