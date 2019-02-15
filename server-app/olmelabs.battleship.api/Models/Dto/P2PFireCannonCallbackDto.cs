using Newtonsoft.Json;

namespace olmelabs.battleship.api.Models.Dto
{
    public class P2PFireCannonCallbackDto
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        [JsonProperty("cellId")]
        public int CellId { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }

        [JsonProperty("ship")]
        public int[] ShipDestroyed { get; set; }
        
        [JsonProperty("gameover")]
        public bool IsGameOver { get; set; }
    }
}
