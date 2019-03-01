import * as consts from "../helpers/const";

export const dummy = () => ({
  type: consts.AJAX_CALL_START
});

export const setGameState = (currentState, gameInfo) => ({
  type: consts.SET_GAME_MODE,
  currentState,
  gameInfo
});

export const resetGame = fireResult => ({
  type: consts.RESET_GAME
});

export const makeFire = fireResult => ({
  type: consts.MAKE_FIRE,
  fireResult
});

export const makeFireMultiplayer = fireResult => ({
  type: consts.MAKE_FIRE_MULTIPLAYER,
  fireResult
});

export const cancelLoadingMultiplayer = () => ({
  type: consts.CANCEL_LOADING
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

export const startGameSuccess = () => ({
  type: consts.START_GAME_SUCCESS
});

export const restartGameMultiplayer = () => ({
  type: consts.RESTART_GAME
});
