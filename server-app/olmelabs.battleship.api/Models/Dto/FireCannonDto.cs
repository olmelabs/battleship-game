
using Newtonsoft.Json;

namespace olmelabs.battleship.api.Models.Dto
{
    public class FireCannonDto
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("cellId")]
        public int CellId { get; set; }
    }
}
