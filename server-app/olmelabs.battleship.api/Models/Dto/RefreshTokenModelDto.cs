using Newtonsoft.Json;

namespace olmelabs.battleship.api.Models.Dto
{
    public class RefreshTokenModelDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
