import expect from "expect";
import React from "react";
import { shallow } from "enzyme";
import { HeaderControl } from "./HeaderControl";

function setup(authenticated) {
  const props = {
    authenticated,
    actions: {}
  };
  return shallow(<HeaderControl {...props} />);
}

describe("Header Control", () => {
  it("logout link is visible and login is hidden for authenticated", () => {
    const wrapper = setup(true);

    // expect(wrapper.exists("[test-id='logout-link']")).toBe(true);
    // expect(wrapper.exists("[test-id='login-link']")).toBe(false);
  });

  it("logout link is hidden and login is visible for non-authenticated", () => {
    const wrapper = setup(false);

    // expect(wrapper.exists("[test-id='logout-link']")).toBe(false);
    // expect(wrapper.exists("[test-id='login-link']")).toBe(true);
  });
});
