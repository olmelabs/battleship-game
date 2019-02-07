import delay from "./delay";
import * as consts from "../helpers/const";

export const API_MODE = consts.ApiMode.MOCK;

const gameIdOnServer = "0000000000000000000";
class GameApi {
  static validateBoard(ships) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        resolve(Object.assign({}, { result: true }));
      }, delay);
    });
  }

  static generateBoard() {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        resolve(
          Object.assign(
            {},
            {
              ships: [
                { title: "4X", vertical: false, cells: [5, 6, 7, 8], hits: 0 },
                { title: "3X", vertical: true, cells: [22, 32, 42], hits: 0 },
                { title: "3X", vertical: true, cells: [57, 67, 77], hits: 0 },
                { title: "2X", vertical: true, cells: [26, 36], hits: 0 },
                { title: "2X", vertical: true, cells: [30, 40], hits: 0 },
                { title: "2X", vertical: true, cells: [24, 34], hits: 0 },
                { title: "1X", vertical: true, cells: [83], hits: 0 },
                { title: "1X", vertical: false, cells: [1], hits: 0 },
                { title: "1X", vertical: true, cells: [28], hits: 0 },
                { title: "1X", vertical: true, cells: [65], hits: 0 }
              ]
            }
          )
        );
      }, delay);
    });
  }

  static startNewGame(connectionId) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        resolve(Object.assign({}, { gameId: gameIdOnServer }));
      }, delay);
    });
  }

  static stopGame(gameId) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        resolve(
          Object.assign(
            {},
            {
              ships: [
                [26, 36, 46, 56],
                [22, 23, 24],
                [18, 28, 38],
                [51, 61],
                [73, 83],
                [78, 88],
                [53],
                [85],
                [81],
                [30]
              ]
            }
          )
        );
      }, delay);
    });
  }

  static fireCannon(shotData) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        switch (shotData.cellId) {
          case 48:
          case 58:
          case 68:
          case 78:
            resolve({
              cellId: shotData.cellId,
              result: true,
              ship: [48, 58, 68, 78],
              gameover: false,
              serverturn: false
            });
            break;

          case 99:
            resolve({
              cellId: shotData.cellId,
              result: true,
              ship: [99],
              gameover: true,
              serverturn: false
            });
            break;

          default:
            resolve({
              cellId: shotData.cellId,
              result: shotData.cellId % 2 == 0,
              gameover: false,
              serverturn: false
            });
        }
      }, delay);
    });
  }

  static fireCannonResponse(shotResult) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        resolve({});
      }, delay);
    });
  }

  static login(email, password) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        if (email === "user@domain.com" && password === "password") {
          resolve(
            Object.assign(
              {},
              {
                email: "user@domain.com",
                firstName: "Test",
                lastName: "User",
                token: "tokenString"
              }
            )
          );
        } else {
          reject("Unauthorised");
        }
      }, delay);
    });
  }

  static register(user) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        if (user.email === "ok@domain.com" && user.password === "123") {
          resolve(Object.assign({}, { success: true, message: null }));
        } else {
          resolve(
            Object.assign(
              {},
              { success: false, message: "This email is already taken." }
            )
          );
        }
      }, delay);
    });
  }

  static sendResetPasswordLink(email) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        resolve(Object.assign({}, { success: true }));
      }, delay);
    });
  }

  static resetPassword(code, password, password2) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        if (code === "IDDQD") {
          resolve(Object.assign({}, { success: true, message: null }));
        } else {
          resolve(
            Object.assign(
              {},
              { success: false, message: "Reset password failed." }
            )
          );
        }
      }, delay);
    });
  }

  static confirmEmail(code) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        if (code === "IDDQD") {
          resolve(Object.assign({}, { success: true, message: null }));
        } else {
          resolve(
            Object.assign(
              {},
              { success: false, message: "Something went wrong." }
            )
          );
        }
      }, delay);
    });
  }

  static startSession(connectionId) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        resolve(Object.assign({}, { code: "12345" }));
      }, delay);
    });
  }

  static joinSession(code, connectionId) {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        resolve({});
      }, delay);
    });
  }
}

export default GameApi;
