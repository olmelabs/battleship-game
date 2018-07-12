namespace olmelabs.battleship.api.Models.Entities
{
    public class FireResult
    {
        public bool IsHit { get; set; }

        public ShipInfo ShipDestroyed { get; set; }

        public bool IsGameOver { get; set; }
    }
}
