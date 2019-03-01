import { API_URL } from "./api.config";
import * as consts from "../helpers/const";

export const API_MODE = consts.ApiMode.WEB;

class GameApi {
  static validateBoard(ships) {
    return postDataReturnJsonWithRefreshAsync("Game/ValidateBoard/", ships);
  }

  static generateBoard() {
    return postDataReturnJsonWithRefreshAsync("Game/GenerateBoard/");
  }

  static startSinglePlayerNewGame(connectionId) {
    return postDataReturnJsonWithRefreshAsync(
      "Game/StartNewGame/",
      connectionId
    );
  }

  static stopSinglePlayerGame(gameId) {
    return postDataReturnJsonWithRefreshAsync("Game/StopGame/", gameId);
  }

  static fireCannon(shotData) {
    return postDataReturnJsonWithRefreshAsync("Game/FireCannon/", shotData);
  }

  static fireCannonResponse(shotResult) {
    return postDataReturnJsonWithRefreshAsync(
      "Game/FireCannonProcessResult",
      shotResult
    );
  }

  static login(email, password) {
    return postDataReturnJson("Auth/Token", { email, password });
  }

  static register(user) {
    return postDataReturnJson("Account/Register", user);
  }

  static sendResetPasswordLink(email) {
    return postDataReturnJson("Account/SendResetPasswordLink", email);
  }

  static resetPassword(code, password, password2) {
    return postDataReturnJson("Account/ResetPassword", {
      code,
      password,
      password2
    });
  }

  static confirmEmail(code) {
    return postDataReturnJson("Account/ConfirmEmail", code);
  }

  static startSession(connectionId) {
    return postDataReturnJsonWithRefreshAsync(
      "PeerToPeerGame/StartSession",
      connectionId
    );
  }

  static joinSession(code, connectionId) {
    return postDataReturnJsonWithRefreshAsync("PeerToPeerGame/JoinSession", {
      code,
      connectionId
    });
  }

  static startNewGameMultiplayer(gameInfo) {
    return postDataReturnJsonWithRefreshAsync(
      "PeerToPeerGame/StartNewGame/",
      gameInfo
    );
  }

  static fireCannonMultiplayer(shotData) {
    return postDataReturnJsonWithRefreshAsync(
      "PeerToPeerGame/FireCannon/",
      shotData
    );
  }

  static fireCannonResponseMultiplayer(shotResult) {
    return postDataReturnJsonWithRefreshAsync(
      "PeerToPeerGame/FireCannonProcessResult",
      shotResult
    );
  }

  static reStartGameMultiplayer(code, connectionId) {
    return postDataReturnJsonWithRefreshAsync("PeerToPeerGame/RestartGame", {
      code,
      connectionId
    });
  }
}

//https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch
function postData(url, data) {
  // Default options are marked with *
  return fetch(API_URL + url, {
    body: JSON.stringify(data), // must match 'Content-Type' header
    cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
    credentials: "same-origin", // include, same-origin, *omit
    headers: addHeaders(),
    method: "POST", // *GET, POST, PUT, DELETE, etc.
    mode: "cors", // no-cors, cors, *same-origin
    redirect: "follow", // manual, *follow, error
    referrer: "no-referrer" // *client, no-referrer
  });
}

function postDataReturnJson(url, data) {
  return postData(url, data).then(response => response.json());
}

async function postDataAsync(url, data) {
  // Default options are marked with *
  return await fetch(API_URL + url, {
    body: JSON.stringify(data), // must match 'Content-Type' header
    cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
    credentials: "same-origin", // include, same-origin, *omit
    headers: addHeaders(),
    method: "POST", // *GET, POST, PUT, DELETE, etc.
    mode: "cors", // no-cors, cors, *same-origin
    redirect: "follow", // manual, *follow, error
    referrer: "no-referrer" // *client, no-referrer
  });
}

async function postDataReturnJsonWithRefreshAsync(url, data) {
  let response = await postDataAsync(url, data);
  if (response.ok) {
    return response.json();
  }
  if (response.status === 400) {
    throw new Error(response.status);
  }
  if (response.status === 404) {
    throw new Error(response.status);
  }
  if (response.status === 401) {
    let res = await refreshToken();
    if (res) {
      response = await postDataAsync(url, data);
      if (response.ok) {
        return response.json();
      } else {
        return response.blob;
      }
    } else {
      throw new Error(response.status);
    }
  }
  return response.blob;
}

async function refreshToken() {
  const user = JSON.parse(localStorage.getItem("user"));
  let response = await postDataAsync("Auth/Refresh", {
    token: user.token,
    refreshToken: user.refreshToken
  });
  if (response.ok) {
    let newUser = await response.json();
    localStorage.setItem("user", JSON.stringify(newUser));
    return true;
  }
  if (response.status === 401) {
    return false;
  }
}

function addHeaders() {
  let headers = {
    Accept: "application/json",
    "Content-Type": "application/json"
  };

  let auth = addAuthHeader();
  if (auth !== null) {
    headers = Object.assign({}, headers, auth);
  }

  return headers;
}

function addAuthHeader() {
  const user = JSON.parse(localStorage.getItem("user"));
  if (user && user.token) {
    return { Authorization: "Bearer " + user.token };
  } else {
    return null;
  }
}

export default GameApi;
