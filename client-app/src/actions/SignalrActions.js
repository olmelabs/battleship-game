import * as consts from "../helpers/const";

export function signalrConnected(connectionId) {
  return { type: consts.SIGNALR_ON_CONNECTED, connectionId };
}
