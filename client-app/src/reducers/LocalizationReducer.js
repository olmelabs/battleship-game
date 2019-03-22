import * as consts from "../helpers/const";

const initialState = {
  languageCode: localStorage.getItem("localizationState.languageCode") || "en"
};

const localizationState = (state = initialState, action) => {
  if (action.type == consts.SET_LANGUAGE) {
    return { ...state, languageCode: action.languageCode };
  }

  return state;
};

export default localizationState;
