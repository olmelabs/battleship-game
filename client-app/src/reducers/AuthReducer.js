import * as consts from '../helpers/const';

const initialState = {
  authenticated: false,
  loginFailed: false,
  registrationDone: false,
  registrationSuccess: false,
  registrationMessage: null
};

const authState = (state = initialState, action) => {
  switch (action.type) {

    case consts.ACCOUNT_LOGIN_FAILED:
      return  Object.assign({}, state, {loginFailed: true, authenticated: false});

    case consts.ACCOUNT_LOGIN_SUCCESS:
      return  Object.assign({}, state, {loginFailed: false, authenticated: true});

    case consts.ACCOUNT_LOGOUT:
      return  Object.assign({}, state, {loginFailed: false, authenticated: false});

    case consts.ACCOUNT_REGISTER:
      return Object.assign({}, state, {registrationDone: true, registrationSuccess: action.data.success, registrationMessage: action.data.message});

    case consts.ACCOUNT_REGISTER_RESET:
      return Object.assign({}, state, {registrationDone: false, registrationSuccess: false, registrationMessage: null});
  }

  return state;
};

export default authState;
