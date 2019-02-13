using System;
using System.Collections.Generic;

namespace olmelabs.battleship.api.Models.Entities
{
    //TODO: After implementation look at Game State - may be everything may be merged into one class... 
    //and SignalR data maty be moved to GameSessionState class. For now just duplicate States to see how it goes

    public class PeerToPeerGameState : BsonBase
    {
        public static PeerToPeerGameState CreateNew()
        {
            PeerToPeerGameState g = new PeerToPeerGameState()
            {
                GameId = Guid.NewGuid().ToString(),
                FriendsBoard = new BoardInfo(),
                HostBoard = new BoardInfo(),
                DateStart = DateTime.Now
            };

            return g;
        }

        public PeerToPeerGameState()
        {
            CurrentShipHost = new List<int>();
            CurrentShipFriend = new List<int>();
        }

        public string GameId { get; set; }

        public BoardInfo FriendsBoard { get; set; }

        public BoardInfo HostBoard { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public List<int> CurrentShipHost { get; set; }

        public List<int> CurrentShipFriend { get; set; }

        public bool IsAwaitingFriendsTurn { get; set; }
    }
}
