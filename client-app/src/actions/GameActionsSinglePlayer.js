import * as consts from "../helpers/const";
import * as simpleActions from "./GameActionsSimple";

import gameApi from "../api/api";
import {
  ajaxCallStart,
  ajaxCallErrorCheckAuth,
  ajaxCallSuccess
} from "./AjaxStatusActions";

export function startSinglePlayerNewGame() {
  return function(dispatch, getState) {
    dispatch(ajaxCallStart());
    const ships = getState().gameState.myShips;

    return gameApi
      .validateBoard(ships)
      .then(ret => {
        if (ret.result === true) {
          dispatch(simpleActions.boardValid());

          const connectionId = getState().signalrState.connectionId;
          gameApi.startSinglePlayerNewGame(connectionId).then(gameInfo => {
            dispatch(ajaxCallSuccess());

            dispatch(
              simpleActions.setGameState(consts.GameState.STARTED, gameInfo)
            );
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

export function stopSinglePlayerGame() {
  return function(dispatch, getState) {
    dispatch(ajaxCallStart());

    const gameId = getState().gameState.gameId;
    const ships = getState().gameState.myShips;

    return gameApi
      .stopSinglePlayerGame({ gameId, ships })
      .then(gameOverInfo => {
        dispatch(ajaxCallSuccess());

        dispatch(
          simpleActions.setGameState(consts.GameState.COMPLETED, {
            gameInfo: { gameId }
          })
        );

        dispatch(simpleActions.showEnemyShips(gameOverInfo.ships));
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

        dispatch(simpleActions.makeFire(fireResult));

        if (
          fireResult.ship.constructor === Array &&
          fireResult.ship.length > 0
        ) {
          dispatch(simpleActions.shipDestroyed(fireResult));
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
    dispatch(simpleActions.fireRequestFromServer(fireRequest));

    const gameState = getState().gameState;

    if (gameState.currentState != consts.GameState.STARTED) {
      return;
    }
    const cellId = fireRequest.cellId;
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
