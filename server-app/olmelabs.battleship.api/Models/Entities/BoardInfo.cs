using System.Collections.Generic;

namespace olmelabs.battleship.api.Models.Entities
{
    public class BoardInfo
    {
        public BoardInfo()
        {
            Board = new int[100];
            Ships = new List<ShipInfo>(10);
        }

        public int[] Board { get; set; }

        public List<ShipInfo> Ships { get; set; }
    }
}
