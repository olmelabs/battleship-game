using Newtonsoft.Json;

namespace olmelabs.battleship.api.Models.Dto
{
    public class P2PNewGametDto
    {
        public string Code { get; set; }

        public string ConnectionId { get; set; }

        public ClientShipDto[] Ships { get; set; }
    }
}
