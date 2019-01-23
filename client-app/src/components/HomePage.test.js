import expect from "expect";
import React from "react";
import { shallow } from "enzyme";
import { HomePage } from "./HomePage";

function setup(authenticated) {
  const props = {
    authenticated
  };
  return shallow(<HomePage {...props} />);
}

describe("Home Page", () => {
  it("login links block is hidden for authenticated", () => {
    const wrapper = setup(true);

    expect(wrapper.exists("p.dev-login-links")).toBe(false);
  });

  it("login links block is visible for not-authenticated", () => {
    const wrapper = setup(false);

    expect(wrapper.exists("p.dev-login-links")).toBe(true);
  });
});
