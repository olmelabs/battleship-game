
using System;
using System.Collections.Generic;

namespace olmelabs.battleship.api.Models.Entities
{
    //TODO: make multiple games available within same session
    public class PeerToPeerSessionState : BsonBase
    {
        public PeerToPeerSessionState()
        {
            PreviousGames = new List<string>();
        }

        public string Code { get; set; }

        public string HostConnectionId { get; set; }

        public bool HostStartedGame { get; set; }

        public List<ShipInfo> HostShips { get; set; }

        public string FriendConnectionId { get; set; }

        public bool FriendStartedGame { get; set; }

        public List<ShipInfo> FriendShips { get; set; }

        public int GameStartedCount = 0;

        public string GameId { get; set; }

        public List<string> PreviousGames { get; private set; }

        internal void Reset()
        {
            HostShips = null;
            HostStartedGame = false;

            FriendShips = null;
            FriendStartedGame = false;

            GameStartedCount = 0;
            GameId = null;
        }
    }
}
