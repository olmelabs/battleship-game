import expect from 'expect';
import React from 'react';
import {shallow} from 'enzyme';
import HomePage from './HomePage';
import { store } from '../helpers/store.prod'; //change to store.dev to see dump of store.
import * as actions from '../actions';

function setup() {
  return shallow(<HomePage store={store}/>).dive();
}

describe('Home Page', () => {

  it('login links block is hidden for authenticated', () => {
    //modify store state to test page props
    store.dispatch(actions.loginSuccess());

    const wrapper = setup();
    expect(wrapper.exists('p.dev-login-links')).toBe(false);
  });

  it('login links block is visible for not-authenticated', () => {
    store.dispatch(actions.loginFailed());

    const wrapper = setup();
    expect(wrapper.exists('p.dev-login-links')).toBe(true);
  });

});
