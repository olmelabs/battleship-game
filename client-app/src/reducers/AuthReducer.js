import * as consts from "../helpers/const";

const initialState = {
  authenticated: localStorage.getItem("user") != null,
  loginFailed: false,
  registrationDone: false,
  registrationSuccess: false,
  registrationMessage: null,
  resetPasswordDone: false,
  resetPasswordSuccess: false,
  resetPasswordMessage: null,
  emailConfirmSuccess: false,
  emailConfirmMessage: null
};

const authState = (state = initialState, action) => {
  switch (action.type) {
    case consts.ACCOUNT_LOGIN_FAILED:
      return Object.assign({}, state, {
        loginFailed: true,
        authenticated: false
      });

    case consts.ACCOUNT_LOGIN_SUCCESS:
      return Object.assign({}, state, {
        loginFailed: false,
        authenticated: true
      });

    case consts.ACCOUNT_LOGOUT:
      return Object.assign({}, state, {
        loginFailed: false,
        authenticated: false
      });

    case consts.ACCOUNT_REGISTER:
      return Object.assign({}, state, {
        registrationDone: true,
        registrationSuccess: action.data.success,
        registrationMessage: action.data.message
      });

    case consts.ACCOUNT_REGISTER_RESET:
      return Object.assign({}, state, {
        registrationDone: false,
        registrationSuccess: false,
        registrationMessage: null
      });

    case consts.ACCOUNT_RESET_PASSWORD:
      return Object.assign({}, state, {
        resetPasswordDone: true,
        resetPasswordSuccess: action.data.success,
        resetPasswordMessage: action.data.message
      });

    case consts.ACCOUNT_CONFIRM_EMAIL:
      return Object.assign({}, state, {
        emailConfirmSuccess: action.data.success,
        emailConfirmMessage: action.data.message
      });
  }

  return state;
};

export default authState;
