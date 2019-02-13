
namespace olmelabs.battleship.api.Models.Entities
{
    public class PeerToPeerSessionState
    {
        public PeerToPeerSessionState()
        {

        }

        public string Code { get; set; }

        public string HostConnectionId { get; set; }

        public int GameStartedCount = 0;

        public string FriendConnectionId { get; set; }

        public bool IsFriendReady { get; set; }

        public string GameId { get; set; }

    }
}
