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

    expect(wrapper.exists("[test-id='authorized-text']")).toBe(true);
    expect(wrapper.exists("[test-id='unauthorized-text']")).toBe(false);
  });

  it("login links block is visible for non-authenticated", () => {
    const wrapper = setup(false);

    expect(wrapper.exists("[test-id='authorized-text']")).toBe(false);
    expect(wrapper.exists("[test-id='unauthorized-text']")).toBe(true);
  });
});
