using System;
using System.Collections.Generic;

namespace olmelabs.battleship.api.Models.Entities
{
    public class GameState : BsonBase
    {
        public static GameState CreateNew(IGameLogic gameLogic)
        {
            BoardInfo boardInfo = gameLogic.GenerateBoad();
            GameState g = new GameState()
            {
                GameId = Guid.NewGuid().ToString(),
                ServerBoard = boardInfo,
                ClientBoard = new BoardInfo(),
                DateStart = DateTime.Now
            };

            return g;
        }

        public GameState()
        {
            CurrentShip = new List<int>();
        }

        /// <summary>
        /// signalr connection id to route game messages to single client
        /// </summary>
        public string ConnectionId { get; set; }

        public string GameId { get; set; }

        public BoardInfo ServerBoard { get; set; }

        public BoardInfo ClientBoard { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public List<int> CurrentShip { get; set; }

        /// <summary>
        /// Whether next move is for server
        /// </summary>
        public bool IsAwaitingServerTurn { get; set; }
    }
}
