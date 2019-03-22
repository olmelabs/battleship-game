/*eslint-disable import/default*/
import "babel-polyfill";
import React from "react";
import { render } from "react-dom";
import { Provider } from "react-redux";
import { store } from "./helpers/store";
import App from "./components/App";
import "./helpers/i18n";

// TODO: Move notifications to more modern package. This conflicts with bootsrap and is old
import "../node_modules/toastr/build/toastr.min.css";
import "./styles/styles.css";

render(
  <Provider store={store}>
    <App />
  </Provider>,
  document.getElementById("app")
);
