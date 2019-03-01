import * as consts from "../helpers/const";
import { updateShip, placeShipOnBoard } from "../services/GameService";

const initialState = {
  currentState: consts.GameState.NOT_STARTED,
  gameType: consts.GameType.SINGLEPLAYER,
  multiplayer: {
    gameAccessCode: null,
    joinGameError: false,
    isFriendConnected: false,
    startGameSuccess: false,
    cancelLoading: false
  },
  gameId: null,
  //server turn in singleplayer game, friends turn in mulitplayer game
  isServerTurn: false,
  myShips: [
    { title: "4X", vertical: false, cells: Array(4).fill(null), hits: 0 },
    { title: "3X", vertical: false, cells: Array(3).fill(null), hits: 0 },
    { title: "3X", vertical: false, cells: Array(3).fill(null), hits: 0 },
    { title: "2X", vertical: false, cells: Array(2).fill(null), hits: 0 },
    { title: "2X", vertical: false, cells: Array(2).fill(null), hits: 0 },
    { title: "2X", vertical: false, cells: Array(2).fill(null), hits: 0 },
    { title: "1X", vertical: false, cells: Array(1).fill(null), hits: 0 },
    { title: "1X", vertical: false, cells: Array(1).fill(null), hits: 0 },
    { title: "1X", vertical: false, cells: Array(1).fill(null), hits: 0 },
    { title: "1X", vertical: false, cells: Array(1).fill(null), hits: 0 }
  ],
  myShipsCurrent: -1,
  myBoard: Array(100).fill(null),
  enemyBoard: Array(100).fill(0),
  lastMyDestroyedShip: null,
  isMyBoardValid: true
};

const gameState = (state = initialState, action) => {
  switch (action.type) {
    case consts.RESET_GAME:
      return initialState;

    case consts.SET_GAME_MODE:
      if (action.currentState === consts.GameState.NOT_STARTED) {
        return initialState;
      } else {
        if (state.gameType == consts.GameType.SINGLEPLAYER) {
          return {
            ...state,
            currentState: action.currentState,
            gameId: action.gameInfo.gameId
          };
        } else
          return {
            ...state,
            currentState: action.currentState,
            gameId: action.gameInfo === null ? null : action.gameInfo.gameId,
            isServerTurn:
              action.gameInfo === null
                ? state.isServerTurn
                : !action.gameInfo.yourMove
          };
      }

    case consts.MAKE_FIRE:
      if (state.currentState === consts.GameState.STARTED) {
        const enemyBoard = state.enemyBoard.slice();
        action.fireResult.result
          ? (enemyBoard[action.fireResult.cellId] = 2)
          : (enemyBoard[action.fireResult.cellId] = 1);
        return Object.assign({}, state, {
          enemyBoard,
          isServerTurn: action.fireResult.serverturn
        });
      }
      return state;

    case consts.MAKE_FIRE_MULTIPLAYER:
      if (state.currentState === consts.GameState.STARTED) {
        const enemyBoard = state.enemyBoard.slice();
        action.fireResult.result
          ? (enemyBoard[action.fireResult.cellId] = 2)
          : (enemyBoard[action.fireResult.cellId] = 1);

        return {
          ...state,
          enemyBoard,
          isServerTurn: action.fireResult.serverturn,
          multiplayer: {
            ...state.multiplayer,
            cancelLoading: true
          }
        };
      }
      return state;

    case consts.CANCEL_LOADING:
      if (state.currentState === consts.GameState.STARTED) {
        return {
          ...state,
          multiplayer: {
            ...state.multiplayer,
            cancelLoading: false
          }
        };
      }
      return state;

    case consts.MAKE_FIRE_SERVER:
      if (state.currentState === consts.GameState.STARTED) {
        const myBoard = state.myBoard.slice();
        const isHit = myBoard[action.fireRequest.cellId] === 1;
        isHit
          ? (myBoard[action.fireRequest.cellId] = 3)
          : (myBoard[action.fireRequest.cellId] = 2);

        if (!isHit) {
          return Object.assign({}, state, {
            myBoard,
            isServerTurn: isHit,
            lastMyDestroyedShip: null
          });
        }

        //increment hits count on ship
        const myShips = state.myShips.slice();
        let newShip = null;
        let numberOfDestroyedShips = 0;
        myShips.map((ship, index) => {
          if (ship.cells.includes(action.fireRequest.cellId)) {
            newShip = Object.assign({}, ship, { hits: ship.hits + 1 });
            myShips[index] = newShip;
          }
        });

        //check ship destoyed
        const lastMyDestroyedShip =
          newShip !== null && newShip.cells.length == newShip.hits
            ? newShip
            : null;

        return Object.assign({}, state, {
          myBoard,
          myShips,
          isServerTurn: isHit,
          lastMyDestroyedShip
        });
      }
      return state;

    case consts.SHIP_DESTROYED:
      if (state.currentState === consts.GameState.STARTED) {
        const enemyBoard = state.enemyBoard.slice();
        if (action.fireResult.ship !== null) {
          action.fireResult.ship.map(cellId => {
            enemyBoard[cellId] += 10;
          });
        }
        return Object.assign({}, state, { enemyBoard: enemyBoard });
      }
      return state;

    case consts.SHOW_ENEMY_SHIPS:
      if (state.currentState === consts.GameState.COMPLETED) {
        const enemyBoard = state.enemyBoard.slice();
        action.ships.map(ship => {
          ship.map(cellId => {
            enemyBoard[cellId] += 10;
          });
        });
        return Object.assign({}, state, { enemyBoard: enemyBoard });
      }
      return state;

    case consts.SHIP_MOVED:
    case consts.SHIP_ROTATED:
      if (state.currentState === consts.GameState.NOT_STARTED) {
        const oldShip = state.myShips[state.myShipsCurrent];
        let shipData = null;
        if (action.data.rotation && oldShip.cells.length > 1) {
          shipData = {
            vertical: !oldShip.vertical
          };
        } else {
          shipData = {
            cell: action.data.cellId
          };
        }
        const newShip = updateShip(oldShip, shipData);
        const otherShips = state.myShips.slice();
        otherShips.splice(state.myShipsCurrent, 1);

        const newBoard = state.myBoard.slice();
        placeShipOnBoard(newBoard, oldShip, newShip, otherShips);

        const newShips = state.myShips.slice();
        newShips[state.myShipsCurrent] = newShip;

        return Object.assign({}, state, {
          myShips: newShips,
          myBoard: newBoard
        });
      }
      return state;

    case consts.SHIP_INDEX_CHANGED:
      if (state.currentState === consts.GameState.NOT_STARTED) {
        return Object.assign({}, state, { myShipsCurrent: action.data.index });
      }
      return state;

    case consts.BOARD_VALID:
      return Object.assign({}, state, { isMyBoardValid: true });

    case consts.BOARD_INVALID:
      return Object.assign({}, state, { isMyBoardValid: false });

    case consts.BOARD_GENERATED:
      if (state.currentState === consts.GameState.NOT_STARTED) {
        let i = 0;
        let newShips = action.board.ships;
        const newBoard = Array(100).fill(null);
        let otherShips = state.myShips.slice();

        newShips.map(newShip => {
          const oldShip = state.myShips[i];
          let otherShipsUpdated = otherShips.slice();
          otherShipsUpdated.splice(i, 1);
          placeShipOnBoard(newBoard, oldShip, newShip, otherShips);
          otherShips[i] = newShip;
          i++;
        });

        return Object.assign({}, state, {
          myShips: newShips,
          myBoard: newBoard
        });
      }
      return state;

    case consts.SET_GAME_TYPE:
      if (state.currentState === consts.GameState.NOT_STARTED) {
        return Object.assign({}, state, {
          gameType: action.gameType
        });
      }
      return state;

    case consts.SET_GAME_CODE:
      if (state.currentState === consts.GameState.NOT_STARTED) {
        return Object.assign({}, state, {
          multiplayer: {
            ...state.multiplayer,
            gameAccessCode: action.gameAccessCode
          }
        });
      }
      return state;

    case consts.JOIN_GAME_ERROR:
      if (state.currentState === consts.GameState.NOT_STARTED) {
        return Object.assign({}, state, {
          multiplayer: {
            ...state.multiplayer,
            joinGameError: true
          }
        });
      }
      return state;

    case consts.JOIN_GAME_SUCCESS:
      if (state.currentState === consts.GameState.NOT_STARTED) {
        return Object.assign({}, state, {
          multiplayer: {
            ...state.multiplayer,
            isFriendConnected: true
          }
        });
      }
      return state;

    case consts.START_GAME_SUCCESS:
      if (state.currentState === consts.GameState.NOT_STARTED) {
        return {
          ...state,
          multiplayer: {
            ...state.multiplayer,
            startGameSuccess: true
          }
        };
      }
      return state;

    case consts.RESTART_GAME: {
      return Object.assign({}, initialState, {
        currentState: consts.GameState.NOT_STARTED,
        gameType: state.gameType,
        multiplayer: {
          ...initialState.multiplayer,
          gameAccessCode: state.multiplayer.gameAccessCode,
          isFriendConnected: true
        }
      });
    }

    default:
      return state;
  }
};

export default gameState;
