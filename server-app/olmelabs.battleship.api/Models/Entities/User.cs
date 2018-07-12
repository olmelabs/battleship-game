using MongoDB.Bson.Serialization.Attributes;

namespace olmelabs.battleship.api.Models.Entities
{
    public class User : BsonBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [BsonIgnore]
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
