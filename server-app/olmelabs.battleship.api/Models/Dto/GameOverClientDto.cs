using System.Collections.Generic;

namespace olmelabs.battleship.api.Models.Dto
{
    public class GameOverClientDto
    {
        public GameOverClientDto()
        {
            List<ClientShipDto> input = new List<ClientShipDto>();
        }

        public string GameId { get; set; }

        public List<ClientShipDto> Ships { get; set; }
    }
}
