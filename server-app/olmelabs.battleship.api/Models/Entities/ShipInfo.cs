
namespace olmelabs.battleship.api.Models.Entities
{
    public class ShipInfo
    {
        public ShipInfo(bool isVertical, int[] cells)
        {
            IsVertical = isVertical;
            Cells = cells;
        }

        public bool IsVertical { get; set; }

        public int[] Cells { get; set; }

        public int Hits { get; set; }
    }
}
