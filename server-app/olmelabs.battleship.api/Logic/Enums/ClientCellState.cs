namespace olmelabs.battleship.api.Logic
{
    public enum ClientCellState
    {
        CellNotFired = 0,
        CellFiredResultNotYetKnown = 1,
        CellFiredButEmpty = 2,
        CellFiredAndShipHit = 3
    }
}
