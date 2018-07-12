using Newtonsoft.Json;

namespace olmelabs.battleship.api.Models.Dto
{
    public class NewGameDto
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }
    }
}
