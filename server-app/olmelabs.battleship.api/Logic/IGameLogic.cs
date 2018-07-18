using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using System.Collections.Generic;

namespace olmelabs.battleship.api
{
    public interface IGameLogic
    {
        BoardInfo GenerateBoad();

        int ChooseNextClientCell(BoardInfo clientBoard, List<int> currentShip, ClientStatistics statistics);
        
        bool ValidateClientBoard(List<ClientShipDto> ships);

        void MarkCellsAroundShip(BoardInfo clientBoard, ShipInfo ship);
    }
}
