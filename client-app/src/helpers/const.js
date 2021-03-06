export const DUMMY = "DUMMY";
export const AJAX_CALL_START = "AJAX_CALL_START";
export const AJAX_CALL_ERROR = "AJAX_CALL_ERROR";
export const AJAX_CALL_SUCCESS = "AJAX_CALL_SUCCESS";

export const SIGNALR_ON_CONNECTED = "SIGNALR_ON_CONNECTED";

export const SET_GAME_MODE = "SET_GAME_MODE";
export const RESET_GAME = "RESET_GAME";
export const MAKE_FIRE = "MAKE_FIRE";
export const MAKE_FIRE_SERVER = "MAKE_FIRE_SERVER";
export const MAKE_FIRE_MULTIPLAYER = "MAKE_FIRE_MULTIPLAYER";
export const SHIP_DESTROYED = "SHIP_DESTROYED";
export const SHOW_ENEMY_SHIPS = "SHOW_ENEMY_SHIPS";
export const SET_MOVE_IN_PROGRESS_MULTIPLAYER =
  "SET_MOVE_IN_PROGRESS_MULTIPLAYER";
export const RESET_MOVE_IN_PROGRESS_MULTIPLAYER =
  "RESET_MOVE_IN_PROGRESS_MULTIPLAYER";

export const SHIP_MOVED = "SHIP_MOVED";
export const SHIP_ROTATED = "SHIP_ROTATED";
export const SHIP_INDEX_CHANGED = "SHIP_INDEX_CHANGED";
export const SHIP_HIGHLIGHT = "SHIP_HIGHLIGHT";
export const BOARD_VALID = "BOARD_VALID";
export const BOARD_INVALID = "BOARD_INVALID";
export const BOARD_GENERATED = "BOARD_GENERATED";

export const SET_GAME_TYPE = "SET_GAME_TYPE";

export const GameType = {
  SINGLEPLAYER: "SINGLEPLAYER",
  HOST: "HOST",
  JOIN: "JOIN"
};

export const SET_GAME_CODE = "SET_GAME_CODE";
export const JOIN_GAME = "JOIN_GAME";
export const JOIN_GAME_SUCCESS = "JOIN_GAME_SUCCESS";
export const JOIN_GAME_ERROR = "JOIN_GAME_ERROR";
export const START_GAME_SUCCESS = "START_GAME_SUCCESS";
export const RESTART_GAME = "RESTART_GAME";
export const LOCK_BOARD = "LOCK_BOARD";

export const GameState = {
  NOT_STARTED: "NOT_STARTED",
  STARTED: "STARTED",
  COMPLETED: "COMPLETED"
};

export const BoardType = {
  MY_BOARD: "MY_BOARD",
  ENEMY_BOARD: "ENEMY_BOARD"
};

export const ACCOUNT_LOGOUT = "ACCOUNT_LOGOUT";
export const ACCOUNT_LOGIN_FAILED = "ACCOUNT_LOGIN_FAILED";
export const ACCOUNT_LOGIN_SUCCESS = "ACCOUNT_LOGIN_SUCCESS";
export const ACCOUNT_REGISTER = "ACCOUNT_REGISTER";
export const ACCOUNT_REGISTER_RESET = "ACCOUNT_REGISTER_RESET";
export const ACCOUNT_RESET_PASSWORD = "ACCOUNT_RESET_PASSWORD";
export const ACCOUNT_CONFIRM_EMAIL = "ACCOUNT_CONFIRM_EMAIL";

export const ApiMode = {
  WEB: "WEB",
  MOCK: "MOCK"
};

export const SET_LANGUAGE = "SET_LANGUAGE";
