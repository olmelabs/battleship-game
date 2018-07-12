
using System;

namespace olmelabs.battleship.api.Models.Entities
{
    public class RefreshToken : BsonBase
    {
        public string Token { get; set;}

        public string Email { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
