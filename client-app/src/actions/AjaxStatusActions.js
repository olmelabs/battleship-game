import * as consts from "../helpers/const";

export function ajaxCallStart() {
  return { type: consts.AJAX_CALL_START };
}

export function ajaxCallError() {
  return { type: consts.AJAX_CALL_ERROR };
}

export function ajaxCallSuccess() {
  return { type: consts.AJAX_CALL_SUCCESS };
}
