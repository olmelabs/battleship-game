using Microsoft.VisualStudio.TestTools.UnitTesting;
using olmelabs.battleship.api.Logic;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace olmelabs.battleship.api.tests.Logic
{
    [TestClass]
    public class GameLogicTests
    {
   
        [TestMethod]
        public void GenerateBoadTest()
        {
            IGameLogic gameLogic = new GameLogic();
            BoardInfo boardInfo = gameLogic.GenerateBoad();

            Assert.AreEqual(100, boardInfo.Board.Length);
            Assert.AreEqual(20, boardInfo.Board.Count(x => x == 1));
        }

        [TestMethod]
        public void GenerateShipTest()
        {
            //GameLogic gameLogic = new GameLogic();

            //for (int i = 0; i < 1000; i++)
            //{
            //    ShipInfo shipInfo = gameLogic.GenerateShip(4);

            //    Assert.IsFalse(shipInfo.Cells.Any(x => x > 99));
            //}
        }

        #region ValidateClientBoard tests

        [TestMethod]
        public void ValidateClientBoard_Corners_Vertical_Ok()
        {
            GameLogic g = new GameLogic();

            List<ClientShipDto> input = new List<ClientShipDto>()
            {
                new ClientShipDto { IsVertical = true, Cells = new int?[] { 0, 10, 20, 30 } },
                new ClientShipDto { IsVertical = true, Cells = new int?[] { 9, 19, 29 } },
                new ClientShipDto { IsVertical = true, Cells = new int?[] { 70, 80, 90 } },
                new ClientShipDto { IsVertical = true, Cells = new int?[] { 89, 99 } },
                new ClientShipDto { IsVertical = true, Cells = new int?[] { 45, 55 } },
                new ClientShipDto { IsVertical = true, Cells = new int?[] { 5, 15 } },
                new ClientShipDto { IsVertical = true, Cells = new int?[] { 85 } },
                new ClientShipDto { IsVertical = true, Cells = new int?[] { 93 } },
                new ClientShipDto { IsVertical = true, Cells = new int?[] { 59 } },
                new ClientShipDto { IsVertical = true, Cells = new int?[] { 50 } }
            };

            bool res = g.ValidateClientBoard(input);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void ValidateClientBoard_Corners_Horizontal_Ok()
        {
            GameLogic g = new GameLogic();

            List<ClientShipDto> input = new List<ClientShipDto>()
            {
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 0, 1, 2, 3 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 90, 91, 92 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 7, 8, 9 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 98, 99 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 50, 51 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 58, 59 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 30 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 35 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 39 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 65 } }
            };

            bool res = g.ValidateClientBoard(input);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void ValidateClientBoard_Mixed_Ok()
        {
            GameLogic g = new GameLogic();

            List<ClientShipDto> input = new List<ClientShipDto>()
            {
                new ClientShipDto { IsVertical = true,  Cells = new int?[] { 1, 11, 21, 31 } },
                new ClientShipDto { IsVertical = true,  Cells = new int?[] { 23, 33, 43 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 36, 37, 38 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 67, 68 } },
                new ClientShipDto { IsVertical = true,  Cells = new int?[] { 72, 82 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 85, 86 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 55 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 8 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 60 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 16 } }
            };

            bool res = g.ValidateClientBoard(input);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void ValidateClientBoard_Fail_Ship_In_TwoLines_1()
        {
            GameLogic g = new GameLogic();

            List<ClientShipDto> input = new List<ClientShipDto>()
            {
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 7, 8, 9, 10 } }, //this ship takes 2 lines
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 23, 24, 25 } }, 
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 43, 44, 45 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 63, 64 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 83, 84 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 88, 89} },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 27 } }, 
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 47 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 67 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 96 } }
            };

            bool res = g.ValidateClientBoard(input);

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void ValidateClientBoard_Fail_Ship_In_TwoLines_2()
        {
            GameLogic g = new GameLogic();

            List<ClientShipDto> input = new List<ClientShipDto>()
            {
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 6, 7, 8, 9 } }, 
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 23, 24, 25 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 43, 44, 45 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 63, 64 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 83, 84 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 89, 90 } }, //this ship takes 2 lines
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 27 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 47 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 67 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 96 } }
            };

            bool res = g.ValidateClientBoard(input);

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void ValidateClientBoard_Fail_Ship_OutOfBoard()
        {
            GameLogic g = new GameLogic();

            List<ClientShipDto> input = new List<ClientShipDto>()
            {
                new ClientShipDto { IsVertical = false, Cells = new int?[] { -10, 0, 10, 20 } }, //this out of board
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 23, 24, 25 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 43, 44, 45 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 63, 64 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 83, 84 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 88, 89 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 27 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 47 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 67 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 96 } }
            };

            bool res = g.ValidateClientBoard(input);

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void ValidateClientBoard_Fail_Ships_TouchEachOther()
        {
            GameLogic g = new GameLogic();

            List<ClientShipDto> input = new List<ClientShipDto>()
            {
                new ClientShipDto { IsVertical = true, Cells = new int?[]  { 0, 10, 20, 30 } }, 
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 21, 22, 23 } }, //touches 4X ship 
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 43, 44, 45 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 63, 64 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 83, 84 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 88, 89 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 27 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 47 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 67 } },
                new ClientShipDto { IsVertical = false, Cells = new int?[] { 96 } }
            };

            bool res = g.ValidateClientBoard(input);

            Assert.IsFalse(res);
        }
        #endregion

        #region MarkCellsAroundShip tests
        [TestMethod]
        public void MarkCellsAroundShip_Vertical_Ship_In_The_Middle()
        {
            GameLogic g = new GameLogic();

            BoardInfo board = new BoardInfo();
            ShipInfo ship = new ShipInfo(true, new[] { 34, 44, 54, 64 });
            int[] expected = new[] { 23, 24, 25, 33, 35, 43, 45, 53, 55, 63, 65, 73, 74, 75 };

            g.MarkCellsAroundShip(board, ship);

            Assert.AreEqual(14, board.Board.Count(x => x == 2));

            //select indexes of cells marked arounf ship
            int[] result = board.Board.Select((val, idx) => new {V = val, I = idx }).Where(x => x.V == 2).Select(x => x.I).ToArray();

            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void MarkCellsAroundShip_Vertical_Ship_In_The_LeftMost_Column()
        {
            GameLogic g = new GameLogic();

            BoardInfo board = new BoardInfo();
            ShipInfo ship = new ShipInfo(true, new[] { 30, 40, 50, 60 });
            int[] expected = new[] { 20, 21, 31, 41, 51, 61, 70, 71 };

            g.MarkCellsAroundShip(board, ship);

            Assert.AreEqual(8, board.Board.Count(x => x == 2));

            //select indexes of cells marked arounf ship
            int[] result = board.Board.Select((val, idx) => new { V = val, I = idx }).Where(x => x.V == 2).Select(x => x.I).ToArray();

            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void MarkCellsAroundShipTest_Vertical_Ship_In_The_RightMost_Column()
        {
            GameLogic g = new GameLogic();

            BoardInfo board = new BoardInfo();
            ShipInfo ship = new ShipInfo(true, new[] { 39, 49, 59, 69 });
            int[] expected = new[] { 28, 29, 38, 48, 58, 68, 78, 79 };

            g.MarkCellsAroundShip(board, ship);

            Assert.AreEqual(8, board.Board.Count(x => x == 2));

            //select indexes of cells marked arounf ship
            int[] result = board.Board.Select((val, idx) => new { V = val, I = idx }).Where(x => x.V == 2).Select(x => x.I).ToArray();

            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void MarkCellsAroundShip_Horizontal_Ship_In_The_Middle()
        {
            GameLogic g = new GameLogic();

            BoardInfo board = new BoardInfo();
            ShipInfo ship = new ShipInfo(false, new[] { 31, 32, 33, 34 });
            int[] expected = new[] { 20, 21, 22, 23, 24, 25, 30, 35, 40, 41, 42, 43, 44, 45 };

            g.MarkCellsAroundShip(board, ship);

            Assert.AreEqual(14, board.Board.Count(x => x == 2));

            //select indexes of cells marked arounf ship
            int[] result = board.Board.Select((val, idx) => new { V = val, I = idx }).Where(x => x.V == 2).Select(x => x.I).ToArray();

            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void MarkCellsAroundShipTest_Horizontal_Ship_In_The_TopMost_Column()
        {
            GameLogic g = new GameLogic();

            BoardInfo board = new BoardInfo();
            ShipInfo ship = new ShipInfo(false, new[] { 1, 2, 3, 4 });
            int[] expected = new[] { 0, 5, 10, 11, 12, 13, 14, 15 };

            g.MarkCellsAroundShip(board, ship);

            Assert.AreEqual(8, board.Board.Count(x => x == 2));

            //select indexes of cells marked arounf ship
            int[] result = board.Board.Select((val, idx) => new { V = val, I = idx }).Where(x => x.V == 2).Select(x => x.I).ToArray();

            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void MarkCellsAroundShip_Horizontal_Ship_In_The_Bottom_Column()
        {
            GameLogic g = new GameLogic();

            BoardInfo board = new BoardInfo();
            ShipInfo ship = new ShipInfo(false, new[] { 91, 92, 93, 94 });
            int[] expected = new[] {80, 81, 82, 83, 84, 85, 90, 95 };

            g.MarkCellsAroundShip(board, ship);

            Assert.AreEqual(8, board.Board.Count(x => x == 2));

            //select indexes of cells marked arounf ship
            int[] result = board.Board.Select((val, idx) => new { V = val, I = idx }).Where(x => x.V == 2).Select(x => x.I).ToArray();

            Assert.IsTrue(result.SequenceEqual(expected));
        }
        #endregion 
    }
}
