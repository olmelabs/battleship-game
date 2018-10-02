import expect from 'expect';
import React from 'react';
import {mount, shallow} from 'enzyme';
import {HomePage} from './HomePage';

function setup() {
  const  props = {
    authenticated: true
    // actions: {foo: () => {return Promise.resolve();}},
    // obj: {id: '', title: ''}
  };
  return shallow(<HomePage {...props} />);
}


describe('Home Page', () => {

  it('sample test', () => {
    const wrapper = setup();
    expect(wrapper.exists('.text-center')).toBe(true);
  });

});
