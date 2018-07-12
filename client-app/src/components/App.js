import React from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { PrivateRoute } from './_PrivateRoute';
import { bindActionCreators } from 'redux';
import {BrowserRouter, Route} from 'react-router-dom';
import { Switch } from 'react-router';
import GamePage from './GamePage';
import LoginPage from './LoginPage';
import HomePage  from './HomePage';
import ProfilePage from './ProfilePage';
import RegisterPage from './RegisterPage';
import HeaderControl from './HeaderControl';
import FooterControl from './FooterControl';
import _NotFound from './_NotFound';

import toastr from 'toastr';
import * as actions from '../actions';

import SignalRService from '../services/SignalrService';

class App extends React.Component {
  constructor(props) {
    super(props);

    SignalRService.registerConnection(connectionId => {
      toastr.info("Connected to Server");
      this.props.actions.signalrConnected(connectionId);
    });

    SignalRService.registerFireFromServer(message => {
      toastr.info("" + message.cellId); //"0" digit is ot displayed, so cast to string ;)
      this.props.actions.fireCannonFromServer(message);
    });
  }

  render() {
    return(

      <BrowserRouter>
        <React.Fragment>
          <HeaderControl />
          <main role="main">
            <div  className="container">
                <Switch>
                  <Route exact path="/" component={HomePage} />
                  <Route exact path="/login" component={LoginPage} />
                  <Route exact path="/register" component={RegisterPage} />
                  <Route exact path="/game" component={GamePage} />
                  <PrivateRoute exact path="/profile" component={ProfilePage} />
                  <Route component={_NotFound} />
                </Switch>
            </div>
          </main>
          <FooterControl/>
        </React.Fragment>
      </BrowserRouter>
    );
  }
}

App.propTypes = {
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
});

function mapDispatchToProps(dispatch){
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(App);
