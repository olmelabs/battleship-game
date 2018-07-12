import * as consts from '../helpers/const';
import gameApi from '../api/api';
import {ajaxCallStart, ajaxCallError, ajaxCallSuccess} from './AjaxStatusActions';

export function loginFailed() {
  return { type: consts.ACCOUNT_LOGIN_FAILED };
}
export function loginSuccess() {
  return { type: consts.ACCOUNT_LOGIN_SUCCESS };
}
export function registrationResult(data){
  return { type: consts.ACCOUNT_REGISTER, data };
}
export function registrationReset(){
  return { type: consts.ACCOUNT_REGISTER_RESET};
}

export function login(email, password) {
  return function(dispatch, getState){

    dispatch(ajaxCallStart());

    return gameApi.login(email, password).then(user => {
      dispatch(ajaxCallSuccess());
      if (user && user.token) {
        localStorage.setItem('user', JSON.stringify(user));
      }
      dispatch(loginSuccess());
    }).catch(error => {
      dispatch(ajaxCallError(error));
      dispatch(loginFailed());
    });

  };
}

export  function logout() {
  localStorage.removeItem('user');
  return { type: consts.ACCOUNT_LOGOUT };
}

export  function register(user) {
  return function(dispatch, getState){

    dispatch(ajaxCallStart());

    return gameApi.register(user).then(data => {
      dispatch(ajaxCallSuccess());
      dispatch(registrationResult(data));
    }).catch(error => {
      dispatch(ajaxCallError(error));
      dispatch(registrationResult({success:false, message:'Unknown Error'}));
    });

  };
}



