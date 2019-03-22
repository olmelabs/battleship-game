import * as consts from "../helpers/const";

export const setLanguage = languageCode => ({
  type: consts.SET_LANGUAGE,
  languageCode
});
