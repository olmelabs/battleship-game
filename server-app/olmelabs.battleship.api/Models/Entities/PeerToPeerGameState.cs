
namespace olmelabs.battleship.api.Models.Entities
{
    public class PeerToPeerGameState
    {
        public PeerToPeerGameState()
        {

        }

        public string Code { get; set; }
        public string HostConnectionId { get; set; }
        public int GameStartedCount = 0;
        public string FriendConnectionId { get; set; }
        public bool IsFriendReady { get; set; }
    }
}
