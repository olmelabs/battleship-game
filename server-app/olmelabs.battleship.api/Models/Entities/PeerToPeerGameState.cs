using olmelabs.battleship.api.Models.Dto;
using System;
using System.Collections.Generic;

namespace olmelabs.battleship.api.Models.Entities
{
    //TODO: After implementation look at Game State - may be everything may be merged into one class... 
    //and SignalR data maty be moved to GameSessionState class. For now just duplicate States to see how it goes
    //or it hust can be deleted for multiplayer

    public class PeerToPeerGameState : BsonBase
    {
        public static PeerToPeerGameState CreateNew()
        {
            PeerToPeerGameState g = new PeerToPeerGameState()
            {
                GameId = Guid.NewGuid().ToString(),
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

        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public List<int> CurrentShipHost { get; set; }

        public List<int> CurrentShipFriend { get; set; }

    }
}
