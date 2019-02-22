using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;
using olmelabs.battleship.api.Services.Interfaces;
using System;
using System.Collections.Generic;
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
            //fallback afer 1000 tries if game is super popular :) 
            string code = await GenerateCode() ?? hostConnectionId;
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

        public async Task<PeerToPeerSessionState> AddPeerToSession(string code, string connectionId, IEnumerable<ShipInfo> ships)
        {
            PeerToPeerSessionState s = await _storage.FindP2PSessionAsync(code);

            if (s == null)
                return null;

            //game already started by both peers
            if (s.GameStartedCount == 2)
                return null;

            if (s.HostConnectionId == connectionId || s.FriendConnectionId == connectionId)
            {
                if (s.HostConnectionId == connectionId)
                {
                    s.HostShips = new List<ShipInfo>(ships);
                }
                else if (s.FriendConnectionId == connectionId)
                {
                    s.FriendShips = new List<ShipInfo>(ships);
                }
                
                s.GameStartedCount++;

                s = await _storage.UpdateP2PSessionAsync(s);

                return s;
            }

            return null;
        }

        public virtual async Task<PeerToPeerSessionState> StartNewGameAsync(PeerToPeerSessionState session)
        {
            PeerToPeerGameState game = PeerToPeerGameState.CreateNew();
            session.GameId = game.GameId;

            //TODO: Do we need separate game or session here? Merge objects(?)
            game = await _storage.AddP2PGameAsync(game);
            await _storage.UpdateP2PSessionAsync(session);

            return session;
        }

        public async Task<PeerToPeerSessionState> FindActiveSessionAsync(string code, string connectionId)
        {
            PeerToPeerSessionState s = await _storage.FindP2PSessionAsync(code);

            if (s == null)
                return null;

            if (s.HostConnectionId == connectionId || s.FriendConnectionId == connectionId)
            {
                return s;
            }

            return null;
        }

        private async Task<string> GenerateCode()
        {
            Random rnd = new Random();
            int i = 0;
            while (i < 1000)
            {
                int num = rnd.Next(0, 9999);
                string code = num.ToString().PadLeft(4, '0');

                //if session not found - code OK. Otherwise generate new code
                PeerToPeerSessionState s = await _storage.FindP2PSessionAsync(code);
                if (s == null)
                    return code;
                ++i;
            }
            return null;
        }
    }
}
