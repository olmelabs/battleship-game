import * as consts from "../helpers/const";
import { logout } from "./AuthActions";

export function ajaxCallStart() {
  return { type: consts.AJAX_CALL_START };
}

export function ajaxCallErrorCheckAuth(error) {
  return function(dispatch, getState) {
    if (error.message === "401") {
      dispatch(logout());
    }
    dispatch(ajaxCallError());
  };
}

export function ajaxCallError() {
  return { type: consts.AJAX_CALL_ERROR };
}

export function ajaxCallSuccess() {
  return { type: consts.AJAX_CALL_SUCCESS };
}
