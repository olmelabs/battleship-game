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
  it("game options visible", () => {
    const wrapper = setup(true);

    expect(wrapper.exists("[test-id='homepage-text']")).toBe(true);
  });
});
