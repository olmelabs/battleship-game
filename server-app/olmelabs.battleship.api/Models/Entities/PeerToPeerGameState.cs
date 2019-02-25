using olmelabs.battleship.api.Models.Dto;
using System;
using System.Collections.Generic;

namespace olmelabs.battleship.api.Models.Entities
{
    //TODO: record events on each move so it may be replayed (may be ?)
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

        public string GameId { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }
    }
}
