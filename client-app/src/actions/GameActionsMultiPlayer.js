import * as consts from "../helpers/const";
import * as simpleActions from "./GameActionsSimple";

import gameApi from "../api/api";
import {
  ajaxCallStart,
  ajaxCallErrorCheckAuth,
  ajaxCallSuccess
} from "./AjaxStatusActions";

export function joinGame(code) {
  return function(dispatch, getState) {
    const gameType = getState().gameState.gameType;
    dispatch(ajaxCallStart());

    const connectionId = getState().signalrState.connectionId;

    return gameApi
      .joinSession(code, connectionId)
      .then(res => {
        dispatch(ajaxCallSuccess());
        dispatch(simpleActions.setGameAccessCode(code));
        dispatch(simpleActions.joinGameSuccess());
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        dispatch(simpleActions.joinGameError());
        throw error;
      });
  };
}

export function startNewGameMultiplayer() {
  return function(dispatch, getState) {
    dispatch(ajaxCallStart());
    const ships = getState().gameState.myShips;

    return gameApi
      .validateBoard(ships)
      .then(ret => {
        if (ret.result === true) {
          dispatch(simpleActions.boardValid());

          const code = getState().gameState.multiplayer.gameAccessCode;
          const connectionId = getState().signalrState.connectionId;
          gameApi
            .startNewGameMultiplayer({ code, connectionId, ships })
            .then(gameInfo => {
              dispatch(ajaxCallSuccess());
              dispatch(simpleActions.startGameSuccess());
              //game started by both parties received via SignalR message
            });
        } else {
          dispatch(ajaxCallSuccess());

          dispatch(simpleActions.boardInvalid());
        }
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}

export function reStartNewGameMultiplayer() {
  return function(dispatch, getState) {
    dispatch(simpleActions.restartGameMultiplayer());

    const state = getState();
    const connectionId = state.signalrState.connectionId;
    const code = state.gameState.multiplayer.gameAccessCode;

    dispatch(ajaxCallStart());
    return gameApi
      .reStartGameMultiplayer(code, connectionId)
      .then(res => {
        dispatch(ajaxCallSuccess());
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}

export function fireCannonMultiplayer(cellId) {
  return function(dispatch, getState) {
    if (getState().ajaxState.ajaxCallIsnProgress > 0) {
      return Promise.resolve();
    }
    dispatch(simpleActions.cancelLoadingMultiplayer());
    dispatch(ajaxCallStart());

    const connectionId = getState().signalrState.connectionId;
    const gameAccessCode = getState().gameState.multiplayer.gameAccessCode;

    return gameApi
      .fireCannonMultiplayer({ connectionId, code: gameAccessCode, cellId })
      .then(fireResult => {
        dispatch(ajaxCallSuccess());
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}

export function fireCannonFromServerMultiplayer(fireRequest) {
  return function(dispatch, getState) {
    dispatch(simpleActions.fireRequestFromServer(fireRequest));

    //TODO: merge common code with singleplayer fireCannonFromServer somewhere
    const cellId = fireRequest.cellId;
    const state = getState();
    const gameState = state.gameState;
    const signalrState = state.signalrState;
    const myBoard = gameState.myBoard;
    const ship =
      gameState.lastMyDestroyedShip === null
        ? null
        : gameState.lastMyDestroyedShip.cells;

    //check game over
    let numberOfDestroyedShips = 0;
    gameState.myShips.map((ship, index) => {
      if (ship.hits == ship.cells.length) {
        numberOfDestroyedShips++;
      }
    });

    //quick test to end the game after first shot
    //numberOfDestroyedShips = 10;

    const res = {
      connectionId: signalrState.connectionId,
      code: gameState.multiplayer.gameAccessCode,
      cellId: cellId,
      result: myBoard[cellId] == 1 || myBoard[cellId] == 3,
      ship: ship,
      gameover: numberOfDestroyedShips == 10
    };

    dispatch(ajaxCallStart());
    return gameApi
      .fireCannonResponseMultiplayer(res)
      .then(res => {
        dispatch(ajaxCallSuccess());

        if (numberOfDestroyedShips === 10) {
          //Means your friend won. You will be shown his ships

          // eslint-disable-next-line no-console
          // console.log("GAME OVER YOU LOST");
          dispatch(
            simpleActions.setGameState(consts.GameState.COMPLETED, null)
          );

          dispatch(simpleActions.showEnemyShips(res.ships));
        }
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}

//From SignlaR
export function startGameMultiplayerSrCallback(gameInfo) {
  return function(dispatch, getState) {
    dispatch(simpleActions.setGameState(consts.GameState.STARTED, gameInfo));
  };
}

export function makeFireMultiplayerSrCallback(fireResult) {
  return function(dispatch, getState) {
    dispatch(simpleActions.makeFireMultiplayer(fireResult));

    if (
      fireResult.ship != null &&
      fireResult.ship.constructor === Array &&
      fireResult.ship.length > 0
    ) {
      dispatch(simpleActions.shipDestroyed(fireResult));
    }

    if (fireResult.gameover) {
      //Means you won as all ships on enemy board are destroyed

      // eslint-disable-next-line no-console
      // console.log("GAME OVER YOU WON");
      dispatch(simpleActions.setGameState(consts.GameState.COMPLETED, null));
    }
  };
}
