using Newtonsoft.Json;

namespace olmelabs.battleship.api.Models.Dto
{
    public class P2PNewGameDto
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("yourMove")]
        public bool YourMove { get; set; }
    }
}
