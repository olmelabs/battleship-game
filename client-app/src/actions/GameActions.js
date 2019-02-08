import * as consts from "../helpers/const";
import gameApi from "../api/api";
import {
  ajaxCallStart,
  ajaxCallErrorCheckAuth,
  ajaxCallSuccess
} from "./AjaxStatusActions";

export const dummy = () => ({
  type: consts.AJAX_CALL_START
});

export const setGameState = (currentState, gameId) => ({
  type: consts.SET_GAME_MODE,
  currentState,
  gameId
});

export const makeFire = fireResult => ({
  type: consts.MAKE_FIRE,
  fireResult
});

export const shipDestroyed = fireResult => ({
  type: consts.SHIP_DESTROYED,
  fireResult
});

export const showEnemyShips = ships => ({
  type: consts.SHOW_ENEMY_SHIPS,
  ships
});

export const fireRequestFromServer = fireRequest => ({
  type: consts.MAKE_FIRE_SERVER,
  fireRequest
});

export const shipChanged = data => ({
  type: consts.SHIP_INDEX_CHANGED,
  data
});

export const shipMoved = data => ({
  type: consts.SHIP_MOVED,
  data
});

export const shipRotated = data => ({
  type: consts.SHIP_ROTATED,
  data
});

export const boardValid = () => ({
  type: consts.BOARD_VALID
});

export const boardInvalid = () => ({
  type: consts.BOARD_INVALID
});

export const boardGenerated = board => ({
  type: consts.BOARD_GENERATED,
  board
});

export const setGameType = gameType => ({
  type: consts.SET_GAME_TYPE,
  gameType
});

export const setGameAccessCode = gameAccessCode => ({
  type: consts.SET_GAME_CODE,
  gameAccessCode
});

export const joinGameSuccess = () => ({
  type: consts.JOIN_GAME_SUCCESS
});

export const joinGameError = () => ({
  type: consts.JOIN_GAME_ERROR
});

export function initGameType(gameType) {
  return function(dispatch, getState) {
    dispatch(setGameType(gameType));

    if (gameType === consts.GameType.HOST) {
      dispatch(ajaxCallStart());

      const connectionId = getState().signalrState.connectionId;

      return gameApi
        .startSession(connectionId)
        .then(res => {
          dispatch(ajaxCallSuccess());

          dispatch(setGameAccessCode(res.code));
        })
        .catch(error => {
          dispatch(ajaxCallErrorCheckAuth(error));
          throw error;
        });
    }
  };
}

export function joinGame(code) {
  return function(dispatch, getState) {
    const gameType = getState().gameState.gameType;
    dispatch(ajaxCallStart());

    const connectionId = getState().signalrState.connectionId;

    return gameApi
      .joinSession(code, connectionId)
      .then(res => {
        dispatch(ajaxCallSuccess());
        dispatch(setGameAccessCode(code));
        dispatch(joinGameSuccess());
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        dispatch(joinGameError());
        throw error;
      });
  };
}

export function startMultiPlayerNewGame() {
  return function(dispatch, getState) {
    dispatch(ajaxCallStart());
    const ships = getState().gameState.myShips;

    return gameApi
      .validateBoard(ships)
      .then(ret => {
        if (ret.result === true) {
          dispatch(boardValid());

          const code = getState().gameState.multiplayer.gameAccessCode;
          const connectionId = getState().signalrState.connectionId;
          gameApi.startMultiPlayerNewGame(code, connectionId).then(gameInfo => {
            dispatch(ajaxCallSuccess());

            //dispatch(setGameState(consts.GameState.STARTED, gameInfo.gameId));
          });
        } else {
          dispatch(ajaxCallSuccess());

          dispatch(boardInvalid());
        }
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}

export function startSinglePlayerNewGame() {
  return function(dispatch, getState) {
    dispatch(ajaxCallStart());
    const ships = getState().gameState.myShips;

    return gameApi
      .validateBoard(ships)
      .then(ret => {
        if (ret.result === true) {
          dispatch(boardValid());

          const connectionId = getState().signalrState.connectionId;
          gameApi.startSinglePlayerNewGame(connectionId).then(gameInfo => {
            dispatch(ajaxCallSuccess());

            dispatch(setGameState(consts.GameState.STARTED, gameInfo.gameId));
          });
        } else {
          dispatch(ajaxCallSuccess());

          dispatch(boardInvalid());
        }
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}

export function stopSinglePlayerGame() {
  return function(dispatch, getState) {
    dispatch(ajaxCallStart());

    const gameId = getState().gameState.gameId;
    const ships = getState().gameState.myShips;

    return gameApi
      .stopSinglePlayerGame({ gameId, ships })
      .then(gameInfo => {
        dispatch(ajaxCallSuccess());

        dispatch(setGameState(consts.GameState.COMPLETED, null));

        dispatch(showEnemyShips(gameInfo.ships));
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}

export function fireCannon(cellId) {
  return function(dispatch, getState) {
    if (getState().ajaxState.ajaxCallIsnProgress > 0) {
      return Promise.resolve();
    }

    dispatch(ajaxCallStart());

    const gameId = getState().gameState.gameId;

    return gameApi
      .fireCannon({ gameId, cellId })
      .then(fireResult => {
        dispatch(ajaxCallSuccess());

        dispatch(makeFire(fireResult));

        if (
          fireResult.ship.constructor === Array &&
          fireResult.ship.length > 0
        ) {
          dispatch(shipDestroyed(fireResult));
        }

        if (fireResult.gameover === true) {
          dispatch(stopSinglePlayerGame());
        }
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}

export function fireCannonFromServer(fireRequest) {
  return function(dispatch, getState) {
    dispatch(fireRequestFromServer(fireRequest));

    const cellId = fireRequest.cellId;
    const gameState = getState().gameState;
    const myBoard = getState().gameState.myBoard;
    const ship =
      gameState.lastMyDestroyedShip === null
        ? null
        : gameState.lastMyDestroyedShip.cells;

    const res = {
      gameId: gameState.gameId,
      cellId: cellId,
      result: myBoard[cellId] == 1 || myBoard[cellId] == 3,
      ship: ship
    };

    //check game over
    let numberOfDestroyedShips = 0;
    gameState.myShips.map((ship, index) => {
      if (ship.hits == ship.cells.length) {
        numberOfDestroyedShips++;
      }
    });

    dispatch(ajaxCallStart());

    return gameApi
      .fireCannonResponse(res)
      .then(res => {
        dispatch(ajaxCallSuccess());

        if (numberOfDestroyedShips === 10) {
          dispatch(stopSinglePlayerGame());
        }
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}

export function generateBoard() {
  return function(dispatch, getState) {
    dispatch(ajaxCallStart());

    return gameApi
      .generateBoard()
      .then(board => {
        dispatch(ajaxCallSuccess());

        dispatch(boardGenerated(board));
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}
