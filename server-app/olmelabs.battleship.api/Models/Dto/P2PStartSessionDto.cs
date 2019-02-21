using Newtonsoft.Json;

namespace olmelabs.battleship.api.Models.Dto
{
    public class P2PStartSessionDto
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
