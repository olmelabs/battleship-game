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
import i18n from "../helpers/i18n";

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
        toastr.info(i18n.t("app.connected"));
        this.props.actions.signalrConnected(connectionId);
      });

      SignalRService.registerFireFromServer(data => {
        if (this.props.currentState != consts.GameState.STARTED) {
          return;
        }

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
        toastr.info(i18n.t("app.friendJoined"));
        this.props.actions.joinGameSuccess();
      });

      SignalRService.registerFriendStartedGame(_ => {
        toastr.info(i18n.t("app.friendStartedGame"));
        this.props.actions.joinGameSuccess();
      });

      SignalRService.registerYouStartedGame(_ => {
        toastr.info(i18n.t("app.youStartedGame"));
      });

      SignalRService.registerGameStartedYourMove(data => {
        toastr.info(i18n.t("app.gameStartedYourMove"));
        this.props.actions.startGameMultiplayerSrCallback(data);
      });

      SignalRService.registerGameStartedFriendsMove(data => {
        toastr.info(i18n.t("app.gameStartedFriendMove"));
        this.props.actions.startGameMultiplayerSrCallback(data);
      });

      SignalRService.registerMakeFireProcessResult(data => {
        this.props.actions.fireCannonMultiplayerSrCallback(data);
      });
      SignalRService.registerRestartGame(_ => {
        toastr.info(i18n.t("app.gameRestart"));
        this.props.actions.restartGameMultiplayer();
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
                <Route exact path="/host" component={GamePage} />
                <Route exact path="/join" component={GamePage} />
                <Route exact path="/code" component={JoinGamePage} />
                {/*
                <Route exact path="/login" component={LoginPage} />
                <Route exact path="/register" component={RegisterPage} />
                <PrivateRoute exact path="/profile" component={ProfilePage} />
                */}
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
