import expect from "expect";
import React from "react";
import { Provider } from "react-redux";
import { MemoryRouter } from "react-router-dom";
import { shallow, mount } from "enzyme";
import HomePage from "./HomePage";
import { store } from "../helpers/store.prod"; //change to store.dev to see dump of store.
import * as actions from "../actions";

function setup() {
  const ctx = React.createContext();
  return mount(
    <Provider context={ctx} store={store}>
      <MemoryRouter>
        <HomePage context={ctx} />
      </MemoryRouter>
    </Provider>
  );
}

describe("Home Page Connected", () => {
  it("login links block is hidden for authenticated", () => {
    const wrapper = setup();
    store.dispatch(actions.loginSuccess());

    //console.log(wrapper.html());
    wrapper.update();
    expect(wrapper.exists("p[test-id='login-links']")).toBe(false);
  });

  it("login links block is visible for not-authenticated", () => {
    const wrapper = setup();
    store.dispatch(actions.loginFailed());

    //console.log(wrapper.html());
    wrapper.update();
    expect(wrapper.exists("p[test-id='login-links']")).toBe(true);
  });
});
