using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;
using System;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services
{
    public class PeerToPeerGameService : IPeerToPeerGameService
    {
        private readonly IStorage _storage;
        public PeerToPeerGameService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<PeerToPeerGameState> StartNewSessionAsync(string hostConnectionId)
        {
            //TODO: create some short code here 
            string code = hostConnectionId; 
            PeerToPeerGameState g = new PeerToPeerGameState()
            {
                HostConnectionId = hostConnectionId,
                Code = code
            };

            await _storage.AddP2PGameAsync(g);

            return g;
        }

        public async Task<PeerToPeerGameState> JoinSessionAsync(string code, string connectionId)
        {
            PeerToPeerGameState g = await _storage.FindP2PGameAsync(code);
            if (g == null)
                return null;

            g.FriendConnectionId = connectionId;

            return g;
        }
    }
}
