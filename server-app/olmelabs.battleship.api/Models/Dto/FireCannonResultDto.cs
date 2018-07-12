using Newtonsoft.Json;

namespace olmelabs.battleship.api.Models.Dto
{
    public class FireCannonResultDto
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("cellId")]
        public int CellId { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }

        [JsonProperty("ship")]
        public int[] ShipDestroyed { get; set; }

        [JsonProperty("gameover")]
        public bool IsGameOver { get; set; }

        [JsonProperty("serverturn")]
        public bool IsAwaitingServerTurn { get; set; }
    }
}
