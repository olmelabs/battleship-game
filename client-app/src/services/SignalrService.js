import { SIGNALR_URL } from "../api/api.config";

import { API_MODE } from "../api/api";
import * as consts from "../helpers/const";

import {
  HubConnection,
  HubConnectionBuilder,
  HttpTransportType,
  LogLevel
} from "@aspnet/signalr";

class WebSocketService {
  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl(SIGNALR_URL)
      .configureLogging(LogLevel.Information)
      .build();

    // start connection
    if (API_MODE == consts.ApiMode.WEB) {
      this.connection.start().catch(err => {
        throw err;
      });
    }
  }

  registerConnection(callBack) {
    this.connection.on("AcquireConnectionId", connectionId => {
      callBack(connectionId);
    });
  }

  registerFireFromServer(callBack) {
    this.connection.on("MakeFireFromServer", message => {
      callBack(message);
    });
  }

  registerFriendConnected(callBack) {
    this.connection.on("FriendConnected", message => {
      callBack(message);
    });
  }
}

const SignalRService = new WebSocketService();

export default SignalRService;
