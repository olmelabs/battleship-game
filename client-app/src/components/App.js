import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { PrivateRoute } from "./Common/_PrivateRoute";
import { bindActionCreators } from "redux";
import { BrowserRouter, Route } from "react-router-dom";
import { Switch } from "react-router";
import LoginPage from "./Account/LoginPage";
import ResetPasswordStep1Page from "./Account/ResetPasswordStep1Page";
import ResetPasswordStep2Page from "./Account/ResetPasswordStep2Page";
import ConfirmEmailPage from "./Account/ConfirmEmailPage";
import RegisterPage from "./Account/RegisterPage";
import GamePage from "./Game/GamePage";
import JoinGamePage from "./Game/JoinGamePage";
import HomePage from "./HomePage";
import ProfilePage from "./Account/ProfilePage";
import HeaderControl from "./Common/HeaderControl";
import FooterControl from "./Common/FooterControl";
import _NotFound from "./Common/_NotFound";
import { API_MODE } from "./../api/api";
import * as consts from "./../helpers/const";

import toastr from "toastr";
import * as actions from "../actions";

import SignalRService from "../services/SignalrService";

class App extends React.Component {
  constructor(props) {
    super(props);
  }

  componentDidMount() {
    if (API_MODE === consts.ApiMode.WEB) {
      SignalRService.registerConnection(connectionId => {
        toastr.info("Connected to Server.");
        this.props.actions.signalrConnected(connectionId);
      });

      SignalRService.registerFireFromServer(data => {
        if (this.props.currentState != consts.GameState.STARTED) {
          return;
        }

        toastr.info("" + data.cellId); //"0" digit is not displayed, so cast to string ;)
        if (
          this.props.gameType === consts.GameType.HOST ||
          this.props.gameType === consts.GameType.JOIN
        ) {
          this.props.actions.fireCannonFromServerMultiplayer(data);
        } else {
          this.props.actions.fireCannonFromServer(data);
        }
      });

      SignalRService.registerFriendConnected(_ => {
        toastr.info("Your friend joined the game.");
        this.props.actions.joinGameSuccess();
      });

      SignalRService.registerFriendStartedGame(_ => {
        toastr.info(
          "Your friend started the game. The game will begin when both of you press start."
        );
        this.props.actions.joinGameSuccess();
      });

      SignalRService.registerYouStartedGame(_ => {
        toastr.info(
          "You started the game. The game will begin when both of you press start. Let's wait for your friend."
        );
        this.props.actions.joinGameSuccess();
      });

      SignalRService.registerGameStartedYourMove(data => {
        toastr.success(
          "Your game is now started. You make the first move. Fire!"
        );
        this.props.actions.startGameMultiplayerSrCallback(data);
      });

      SignalRService.registerGameStartedFriendsMove(data => {
        toastr.success(
          "Your game is now started. Your are now waiting for first move from your friend."
        );
        this.props.actions.startGameMultiplayerSrCallback(data);
      });

      SignalRService.registerMakeFireProcessResult(data => {
        this.props.actions.makeFireMultiplayerSrCallback(data);
      });
    }

    if (API_MODE === consts.ApiMode.MOCK) {
      toastr.info("Signal R connection is mocked.");
      this.props.actions.signalrConnected("MOCK_SIGNALR_CONNECTION_ID");
    }
  }

  render() {
    return (
      <BrowserRouter>
        <React.Fragment>
          <HeaderControl />
          <main role="main">
            <div className="container">
              <Switch>
                <Route exact path="/" component={HomePage} />
                <Route exact path="/login" component={LoginPage} />
                <Route exact path="/register" component={RegisterPage} />
                <Route
                  exact
                  path="/password_reset_link"
                  component={ResetPasswordStep1Page}
                />
                <Route
                  exact
                  path="/password_reset"
                  component={ResetPasswordStep2Page}
                />
                <Route
                  exact
                  path="/confirm_email"
                  component={ConfirmEmailPage}
                />
                <Route exact path="/game" component={GamePage} />
                <PrivateRoute exact path="/host" component={GamePage} />
                <PrivateRoute exact path="/join" component={GamePage} />
                <PrivateRoute exact path="/code" component={JoinGamePage} />
                <PrivateRoute exact path="/profile" component={ProfilePage} />
                <Route component={_NotFound} />
              </Switch>
            </div>
          </main>
          <FooterControl />
        </React.Fragment>
      </BrowserRouter>
    );
  }
}

App.propTypes = {
  gameType: PropTypes.string.isRequired,
  currentState: PropTypes.string.isRequired,
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  gameType: state.gameState.gameType,
  currentState: state.gameState.currentState
});

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(App);
