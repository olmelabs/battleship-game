/*eslint-disable import/default*/
import "babel-polyfill";
import React from "react";
import { render } from "react-dom";
import { Provider } from "react-redux";
import { store } from "./helpers/store";
import App from "./components/App";

import "../node_modules/toastr/build/toastr.min.css";
import "./styles/styles.css";

render(
  <Provider store={store}>
    <App />
  </Provider>,
  document.getElementById("app")
);
