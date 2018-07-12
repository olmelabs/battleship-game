using Newtonsoft.Json;

namespace olmelabs.battleship.api.Models.Dto
{
    public class ClientShipDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("vertical")]
        public bool IsVertical { get; set; }

        [JsonProperty("cells")]
        public int?[] Cells { get; set; }

        public int Hits { get; set; }
    }
}
