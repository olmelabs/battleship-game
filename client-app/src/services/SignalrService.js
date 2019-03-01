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

  registerConnection(callback) {
    this.connection.on("AcquireConnectionId", connectionId => {
      callback(connectionId);
    });
  }

  registerFireFromServer(callback) {
    this.connection.on("MakeFireFromServer", message => {
      callback(message);
    });
  }

  registerFriendConnected(callback) {
    this.connection.on("FriendConnected", message => {
      callback(message);
    });
  }

  registerFriendStartedGame(callback) {
    this.connection.on("FriendStartedGame", message => {
      callback(message);
    });
  }

  registerYouStartedGame(callback) {
    this.connection.on("YouStartedGame", message => {
      callback(message);
    });
  }

  registerGameStartedYourMove(callback) {
    this.connection.on("GameStartedYourMove", message => {
      callback(message);
    });
  }

  registerGameStartedFriendsMove(callback) {
    this.connection.on("GameStartedFriendsMove", message => {
      callback(message);
    });
  }

  registerMakeFireProcessResult(callback) {
    this.connection.on("MakeFireProcessResult", message => {
      callback(message);
    });
  }

  registerRestartGame(callback) {
    this.connection.on("RestartGame", message => {
      callback(message);
    });
  }
}

const SignalRService = new WebSocketService();

export default SignalRService;
