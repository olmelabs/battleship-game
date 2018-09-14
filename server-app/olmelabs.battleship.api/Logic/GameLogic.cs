using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace olmelabs.battleship.api.Logic
{
    public class GameLogic : IGameLogic
    {
        Random _random = new Random();

        public virtual BoardInfo GenerateBoad()
        {
            var boardInfo = new BoardInfo();
            ShipInfo shipInfo;

            List<ShipInfo> lst = new List<ShipInfo>();
            for (int i = 4; i > 0; i--)
            {
                for (int j = 0; j <= 4 - i; j++)
                {
                    shipInfo = GenerateShip(i);
                    while (!CanPlaceShipOnBoard(shipInfo, boardInfo.Board))
                    {
                        shipInfo = GenerateShip(i);
                    }

                    PlaceShipOnBoard(shipInfo, boardInfo.Board);
                    boardInfo.Ships.Add(shipInfo);
                }
            }

            return boardInfo;
        }

        public bool ValidateClientBoard(List<ClientShipDto> ships)
        {
            //check 10 ships
            if (ships?.Count != 10)
                return false;

            //check no empty cell indexes
            if (ships.Any(s => s.Cells.Any(c => !c.HasValue)))
                return false;

            //check ship out of board
            if (ships.Any(s => s.Cells.Any(c => c.Value < 0 || c.Value > 99)))
                return false;

            //check ships are not overlapping
            List<int> distinctCells = ships.SelectMany(s => s.Cells.Select(c => c.GetValueOrDefault())).Distinct().ToList();
            if (distinctCells.Count != 20)
                return false;

            int[] board = new int[100];

            foreach (ClientShipDto dto in ships)
            {
                ShipInfo ship = new ShipInfo(dto.IsVertical, dto.Cells.Select(c => c.GetValueOrDefault()).ToArray());

                if (!CanPlaceShipOnBoard(ship, board))
                    return false;

                PlaceShipOnBoard(ship, board);
            }

            return true;
        }

        private ShipInfo GenerateShip(int size)
        {
            bool isVertical = _random.Next(100) >= 50;
            int[] cells = new int[size];

            int x, y;
            if (isVertical)
            {
                x = _random.Next(9);
                y = _random.Next(9 - size);
            }
            else
            {
                x = _random.Next(9 - size);
                y = _random.Next(9);
            }

            //convert x, y to flat index
            int cellId = x + y * 10;

            for (int i = 0; i < size; i++)
            {
                if (isVertical)
                    cells[i] = cellId += 10;
                else
                    cells[i] = cellId += 1;

            }

            return new ShipInfo(isVertical, cells);

        }

        private void PlaceShipOnBoard(ShipInfo shipInfo, int[] board)
        {
            foreach (int i in shipInfo.Cells)
            {
                board[i] = (int)ServerCellState.CellHasShip;
            }
        }

        private bool CanPlaceShipOnBoard(ShipInfo shipInfo, int[] board)
        {
            bool res = true;

            //check all ship cells are empty
            foreach (int i in shipInfo.Cells)
            {
                if (board[i] == (int)ServerCellState.CellHasShip)
                    return false;
            }

            if (shipInfo.IsVertical)
            {
                int topCellIndex = shipInfo.Cells.First();
                int bottomCellIndex = shipInfo.Cells.Last();

                //check ship starts in topmost row or has nothing on the top
                res = res && (topCellIndex <= 9 || board[topCellIndex - 10] == (int)ServerCellState.CellEmpty);

                //check ship ends in very bottom row or has nothing on the bottom
                res = res && (bottomCellIndex >= 90 || board[bottomCellIndex + 10] == (int)ServerCellState.CellEmpty);

                //check top-left cell
                //cell in top row, or cel in the leftmost column, or has top-left cell empty
                res = res && (topCellIndex <= 9 || topCellIndex % 10 == 0 || board[topCellIndex - 11] == (int)ServerCellState.CellEmpty);

                //check top-right cell
                //cell in top row, or cel in the rightmost column, or has top-right cell empty
                res = res && (topCellIndex <= 9 || (topCellIndex + 1) % 10 == 0 ||  board[topCellIndex - 9] == (int)ServerCellState.CellEmpty);

                //check bottom-left cell
                //cell in bottom row, or cell in leftomst column, 
                res = res && (bottomCellIndex >= 90 || bottomCellIndex % 10 == 0 || board[bottomCellIndex + 9] == (int)ServerCellState.CellEmpty);

                //check bottom-right cell
                //cell in bottom row row, or cel in the rightmost column, or has bottom-right cell empty
                res = res && (bottomCellIndex >= 90 || (bottomCellIndex + 1) % 10 == 0 || board[bottomCellIndex + 11] == (int)ServerCellState.CellEmpty);

                //check cells left and right of ship are empty
                foreach (int i in shipInfo.Cells)
                {
                    //if ship is not on the left-most column, check cells from the left are empty 
                    if (i % 10 > 0)
                    {
                        res = res && board[i - 1] == (int)ServerCellState.CellEmpty;
                    }

                    //if ship not in the right-most column, check cells from the right are empty 
                    if ((i + 1) % 10 > 0)
                    {
                        res = res && board[i + 1] == (int)ServerCellState.CellEmpty;
                    }
                }
            }
            else
            {
                int leftCellIndex = shipInfo.Cells.First();
                int rightCellIndex = shipInfo.Cells.Last();

                //check ship fits one line
                if ((leftCellIndex % 10) > (10 - shipInfo.Cells.Length))
                    return false;

                //check ship starts in leftmost cell in row or has nothing on the left 
                res = res && (leftCellIndex % 10 == 0 || board[leftCellIndex - 1] == (int)ServerCellState.CellEmpty);

                //check ship ends in rightmost cell in row or has nothing on the right
                res = res && ((rightCellIndex + 1) % 10 == 0 || board[rightCellIndex + 1] == (int)ServerCellState.CellEmpty);

                //check top-left cell
                //cell in top row, or in leftmost column, or top-left cell is empty
                res = res && (leftCellIndex <= 9 || leftCellIndex % 10 == 0 || board[leftCellIndex - 11] == (int)ServerCellState.CellEmpty);

                //check top-right cell
                //cell in top row, or in rightmost column, or top-right cell is empty
                res = res && (rightCellIndex <= 9 || (rightCellIndex + 1) % 10 == 0 || board[rightCellIndex - 9] == (int)ServerCellState.CellEmpty);

                //check bottom-left cell
                //cell in bottom row, or in leftmost column, or top- ight cell is empty
                res = res && (leftCellIndex > 90 || leftCellIndex % 10 == 0 || board[leftCellIndex + 9] == (int)ServerCellState.CellEmpty);

                //check bottom-right cell
                //cell in bottom row, or in rightmost column, or bottom-right cell is empty
                res = res && (rightCellIndex >= 90 || (rightCellIndex + 1) % 10 == 0 || board[rightCellIndex + 11] == (int)ServerCellState.CellEmpty);

                //check cells above and below ship are empty
                foreach (int i in shipInfo.Cells)
                {
                    //if  ship is not on the first row, check above cells are empty 
                    if (i >= 10)
                    {
                        res = res && board[i - 10] == (int)ServerCellState.CellEmpty;
                    }

                    //if ship not in the last row, check below cells are empty 
                    if (i < 90)
                    {
                        res = res && board[i + 10] == (int)ServerCellState.CellEmpty;
                    }
                }
            }

            return res;
        }

        public virtual int ChooseNextClientCell(BoardInfo clientBoard, List<int> currentShip, ClientStatistics statistics)
        {
            //if no current ship just fire random cell
            if (currentShip.Count == 0)
            {
                //TODO: This is a try to get most frequently used cells from statistics. Comment it for now.
                //if (statistics != null)
                //{
                //    int? cellIndex = GetStatisticalCellIndex(clientBoard.Board, statistics);
                //    if (cellIndex.HasValue)
                //        return cellIndex.Value;
                //}

                //try to guess next cell from most unexpplored space
                int? cell = GetCellIndexFromLongestSpace(clientBoard.Board);
                if (cell.HasValue)
                    return cell.Value;

                return GetRandomCellIndex(clientBoard.Board);
            }
            //if only one cell of ship is marked - try to find if it is horizontal or vertical
            else if (currentShip.Count == 1)
            {
                int i = currentShip[0];

                //add some random decision wheter to try vertical attack first
                bool isVerticalTryFirst = _random.Next(100) >= 50;

                int j;
                if (isVerticalTryFirst)
                {
                    if (TryAttackVertical(clientBoard.Board, i, out j))
                        return j;
                    if (TryAttackHorizontal(clientBoard.Board, i, out j))
                        return j;
                }
                else
                {
                    if (TryAttackHorizontal(clientBoard.Board, i, out j))
                        return j;
                    if (TryAttackVertical(clientBoard.Board, i, out j))
                        return j;
                }

                throw new InvalidOperationException("Either Vertical or Horizontal attack should be possible, as if we have ship not fully destroyed, it shoud be there.");
            }
            //we have more than one cell marked in this ship, so we know if it is horizontal or vertical
            else
            {
                int j;
                currentShip.Sort();
                int i = currentShip.Last();
                int k = currentShip.Skip(currentShip.Count - 2).Take(1).First();
                if (Math.Abs(i - k) == 10) //vertical
                {
                    if (TryAttackVertical(clientBoard.Board, currentShip.Last(), out j))
                        return j;

                    if (TryAttackVertical(clientBoard.Board, currentShip.First(), out j))
                        return j;

                    throw new InvalidOperationException("Vertical attack should be possible. check board");
                }
                else //horizontal
                {
                    if (TryAttackHorizontal(clientBoard.Board, currentShip.Last(), out j))
                        return j;

                    if (TryAttackHorizontal(clientBoard.Board, currentShip.First(), out j))
                        return j;

                    throw new InvalidOperationException("Horizontal attack should be possible. check board");
                }
            }
        }

        private bool TryAttackVertical(int[] board, int i, out int j)
        {
            j = i - 10;
            if (TestCellForFire(board, j))
                return true;

            j = i + 10;
            if (TestCellForFire(board, j))
                return true;

            return false;
        }

        private bool TryAttackHorizontal(int[] board, int i, out int j)
        {
            //leftmost cell - no sense attacking left
            if (i % 10 == 0)
            {
                j = i + 1;
                if (TestCellForFire(board, j))
                    return true;

                return false;
            }
            //rightmost cell - no sense attack from right
            else if ((i + 1) % 10 == 0)
            {
                j = i - 1;
                if (TestCellForFire(board, j))
                    return true;

                return false;
            }
            else
            {
                j = i - 1;
                if (TestCellForFire(board, j))
                    return true;

                j = i + 1;
                if (TestCellForFire(board, j))
                    return true;
            }

            return false;
        }

        private bool TestCellForFire(int[] board, int j)
        {
            if (j < 0 || j > 99)
                return false;

            return board[j] == (int)ClientCellState.CellNotFired;
        }

        private int GetRandomCellIndex(int[] board)
        {
            int i = _random.Next(99);
            while (board[i] != (int)ClientCellState.CellNotFired)
            {
                i = _random.Next(99);
            }
            return i;
        }

        private int? GetStatisticalCellIndex(int[] board, ClientStatistics statistics)
        {
            //TODO: Add Games Count
            var sortedStat = statistics.CellHits.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            for (int i = 0; i < 20; i++)
            {
                int cellIdx = sortedStat.Keys.ElementAt(i);
                if (board[cellIdx] == (int)ClientCellState.CellNotFired)
                    return cellIdx;
            }
            return null;
        }

        public int? GetCellIndexFromLongestSpace(int[] board)
        {
            //we need some cells to be fired before going with intervals
            int firedCellsCount = board.Count(c => c != (int)ClientCellState.CellNotFired);
            double k = firedCellsCount / (double)board.Length;

            if (k < 0.1) //at least 10% of board is known
            {
                return null;
            }

            int longestIntervalStart = -1;
            int longestIntervalEnd = -1;
            bool intervalReset = false;
            int intervalStart = -1;
            int intervalEnd = -1;

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == (int)ClientCellState.CellNotFired)
                {
                    if (intervalReset)
                    {
                        intervalStart = i;
                        intervalReset = false;
                    }
                    intervalEnd = i;
                }
                else
                {
                    if (intervalEnd - intervalStart > longestIntervalEnd - longestIntervalStart)
                    {
                        longestIntervalStart = intervalStart;
                        longestIntervalEnd = intervalEnd;
                    }
                    intervalReset = true;
                }
            }

            if (longestIntervalStart < 0)
                return null;

            return _random.Next(longestIntervalStart, longestIntervalEnd);
        }

        public void MarkCellsAroundShip(BoardInfo clientBoard, ShipInfo ship)
        {
            int cell;
            if (ship.IsVertical)
            {
                cell = ship.Cells.First();
                //if ship is not in the leftmost column
                if (cell % 10 > 0)
                {
                    for (int i = 0; i < ship.Cells.Length; i++)
                    {
                        //mark cells from the left are empty
                        cell = ship.Cells[i];
                        MarkCellAsEmptySafe(clientBoard.Board, cell - 1);
                    }

                    //corner (diagonal) cells on the left
                    cell = ship.Cells.First();
                    MarkCellAsEmptySafe(clientBoard.Board, cell - 11);

                    cell = ship.Cells.Last();
                    MarkCellAsEmptySafe(clientBoard.Board, cell + 9);
                }

                cell = ship.Cells.First();
                //if ship is not in the rightmost column 
                if ((cell + 1) % 10 > 0)
                {
                    for (int i = 0; i < ship.Cells.Length; i++)
                    {
                        //mark cells from the left are empty
                        cell = ship.Cells[i];
                        MarkCellAsEmptySafe(clientBoard.Board, cell + 1);
                    }

                    //corner (diagonal) cells on the right
                    cell = ship.Cells.First();
                    MarkCellAsEmptySafe(clientBoard.Board, cell - 9);

                    cell = ship.Cells.Last();
                    MarkCellAsEmptySafe(clientBoard.Board, cell + 11);
                }


                //cell above first cell
                cell = ship.Cells.First();
                MarkCellAsEmptySafe(clientBoard.Board, cell - 10);

                //cell below last cell
                cell = ship.Cells.Last();
                MarkCellAsEmptySafe(clientBoard.Board, cell + 10);
            }
            else
            {
                //if ship starts not in the left-most column, mark cells from the left are empty 
                cell = ship.Cells.First();
                if (cell % 10 > 0)
                {
                    MarkCellAsEmptySafe(clientBoard.Board, cell - 11);
                    MarkCellAsEmptySafe(clientBoard.Board, cell - 1);
                    MarkCellAsEmptySafe(clientBoard.Board, cell + 9);
                }

                //if ship ends not in the right-most column, mark cells from the right are empty 
                cell = ship.Cells.Last();
                if ((cell + 1) % 10 > 0)
                {
                    MarkCellAsEmptySafe(clientBoard.Board, cell - 9);
                    MarkCellAsEmptySafe(clientBoard.Board, cell + 1);
                    MarkCellAsEmptySafe(clientBoard.Board, cell + 11);
                }

                //for all cells mark cells from top and bottom
                for (int i = 0; i < ship.Cells.Length; i++)
                {
                    cell = ship.Cells[i];
                    MarkCellAsEmptySafe(clientBoard.Board, cell - 10);
                    MarkCellAsEmptySafe(clientBoard.Board, cell + 10);
                }
            }

        }

        private void MarkCellAsEmptySafe(int[] cells, int cell)
        {
            if (cell < 0 || cell > 99)
                return;

            if (cells[cell] == (int)ClientCellState.CellNotFired || cells[cell] == (int)ClientCellState.CellFiredButEmpty)
            {
                cells[cell] = (int)ClientCellState.CellFiredButEmpty;
                return;
            }

            throw new InvalidOperationException("Something went wrong. cell with the ship should not be set as empty");
        }
    }
}
