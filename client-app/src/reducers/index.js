import { combineReducers } from "redux";
import gameState from "./GameReducer";
import ajaxState from "./AjaxReducer";
import signalrState from "./SignalrReducer";
import authState from "./AuthReducer";
import localizationState from "./LocalizationReducer";

export default combineReducers({
  gameState,
  ajaxState,
  authState,
  signalrState,
  localizationState
});
