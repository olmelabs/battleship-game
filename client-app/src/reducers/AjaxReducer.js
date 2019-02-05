import * as consts from "../helpers/const";

const initialState = {
  ajaxCallIsnProgress: 0
};

const ajaxState = (state = initialState, action) => {
  if (action.type == consts.AJAX_CALL_START) {
    return { ajaxCallIsnProgress: state.ajaxCallIsnProgress + 1 };
  } else if (
    action.type == consts.AJAX_CALL_ERROR ||
    action.type == consts.AJAX_CALL_SUCCESS
  ) {
    return { ajaxCallIsnProgress: state.ajaxCallIsnProgress - 1 };
  }

  return state;
};

export default ajaxState;
