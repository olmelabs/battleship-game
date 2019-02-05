import * as consts from "../helpers/const";

const initialState = {
  connectionId: null
};

const signalrState = (state = initialState, action) => {
  if (action.type == consts.SIGNALR_ON_CONNECTED) {
    return { connectionId: action.connectionId };
  }
  return state;
};

export default signalrState;
