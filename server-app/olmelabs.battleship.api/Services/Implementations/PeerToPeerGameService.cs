using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;
using olmelabs.battleship.api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services.Implementations
{
    public class PeerToPeerGameService : IPeerToPeerGameService
    {
        private readonly IStorage _storage;
        public PeerToPeerGameService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<PeerToPeerSessionState> StartNewSessionAsync(string hostConnectionId)
        {
            //TODO: create some short code here 
            string code = hostConnectionId;
            PeerToPeerSessionState s = new PeerToPeerSessionState()
            {
                HostConnectionId = hostConnectionId,
                Code = code
            };

            await _storage.AddP2PSessionAsync(s);

            return s;
        }

        public async Task<PeerToPeerSessionState> JoinSessionAsync(string code, string connectionId)
        {
            PeerToPeerSessionState s = await _storage.FindP2PSessionAsync(code);
            if (s == null)
                return null;

            //if friend already joined.
            if (!string.IsNullOrWhiteSpace(s.FriendConnectionId))
                return null;

            s.FriendConnectionId = connectionId;

            s = await _storage.UpdateP2PSessionAsync(s);

            return s;
        }

        public async Task<PeerToPeerSessionState> AddPeerToSession(string code, string connectionId)
        {
            PeerToPeerSessionState s = await _storage.FindP2PSessionAsync(code);

            if (s == null)
                return null;

            //game already started by both peers
            if (s.GameStartedCount == 2)
                return null;

            if (s.HostConnectionId == connectionId || s.FriendConnectionId == connectionId)
            {
                s.GameStartedCount++;

                s = await _storage.UpdateP2PSessionAsync(s);

                return s;
            }

            return null;
        }

        public virtual async Task<PeerToPeerGameState> StartNewGameAsync(PeerToPeerSessionState session)
        {
            PeerToPeerGameState game = PeerToPeerGameState.CreateNew();
            session.GameId = game.GameId;

            game = await _storage.AddP2PGameAsync(game);
            await _storage.UpdateP2PSessionAsync(session);

            return game;
        }
    }
}
