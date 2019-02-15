
using Newtonsoft.Json;

namespace olmelabs.battleship.api.Models.Dto
{
    public class P2PFireCannonDto
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        [JsonProperty("cellId")]
        public int CellId { get; set; }
    }
}
