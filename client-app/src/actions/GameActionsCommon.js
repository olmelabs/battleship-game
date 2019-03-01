//common actions for single and multiplayer

import * as consts from "../helpers/const";
import * as simpleActions from "./GameActionsSimple";

import gameApi from "../api/api";
import {
  ajaxCallStart,
  ajaxCallErrorCheckAuth,
  ajaxCallSuccess
} from "./AjaxStatusActions";

export function initGameType(gameType) {
  return function(dispatch, getState) {
    dispatch(simpleActions.setGameType(gameType));

    if (gameType === consts.GameType.HOST) {
      dispatch(ajaxCallStart());

      const connectionId = getState().signalrState.connectionId;

      return gameApi
        .startSession(connectionId)
        .then(res => {
          dispatch(ajaxCallSuccess());

          dispatch(simpleActions.setGameAccessCode(res.code));
        })
        .catch(error => {
          dispatch(ajaxCallErrorCheckAuth(error));
          throw error;
        });
    }
  };
}

export function generateBoard() {
  return function(dispatch, getState) {
    dispatch(ajaxCallStart());

    return gameApi
      .generateBoard()
      .then(board => {
        dispatch(ajaxCallSuccess());

        dispatch(simpleActions.boardGenerated(board));
      })
      .catch(error => {
        dispatch(ajaxCallErrorCheckAuth(error));
        throw error;
      });
  };
}
