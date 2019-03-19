import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import GameBoard from "./GameBoard";
import BoardStatus from "./BoardStatus";
import GameMenu from "./GameMenu";
import ShipFactory from "./ShipFactory";
import MoveSemaphore from "./MoveSemaphore";
import * as actions from "../../actions";
import * as consts from "../../helpers/const";
import toastr from "toastr";
import { withRouter, Redirect, Prompt } from "react-router";

class GamePage extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.setGameType = this.setGameType.bind(this);
  }

  componentDidMount() {
    const { location } = this.props;
    if (this.props.connectionId !== null) {
      this.setGameType();
    }
  }

  componentWillReceiveProps(nextProps) {
    if (nextProps.currentState == consts.GameState.COMPLETED) {
      toastr.success("Game Over");
    }
  }

  componentDidUpdate(prevProps) {
    if (prevProps.connectionId === null && this.props.connectionId !== null) {
      this.setGameType();
    }
  }

  componentWillUnmount() {
    this.props.actions.resetGame();
  }

  setGameType() {
    if (location.pathname === "/host") {
      this.props.actions.initGameType(consts.GameType.HOST);
    } else if (location.pathname === "/join") {
      this.props.actions.initGameType(consts.GameType.JOIN);
    } else {
      this.props.actions.initGameType(consts.GameType.SINGLEPLAYER);
    }
  }

  render() {
    if (
      this.props.gameType === consts.GameType.JOIN &&
      this.props.gameAccessCode == null
    ) {
      return <Redirect to="/code" />;
    }

    const enemyBoard =
      this.props.currentState === consts.GameState.STARTED ||
      this.props.currentState === consts.GameState.COMPLETED ? (
        <React.Fragment>
          <GameBoard boardType={consts.BoardType.ENEMY_BOARD} />
          {this.props.currentState === consts.GameState.STARTED && (
            <MoveSemaphore />
          )}
        </React.Fragment>
      ) : (
        <ShipFactory />
      );

    //TODO: move message into separate component as it is growing
    let message = "";
    if (
      this.props.currentState === consts.GameState.NOT_STARTED &&
      this.props.gameType === consts.GameType.HOST &&
      !this.props.isFriendConnected
    ) {
      message = (
        <div className="row">
          <div className="col alert alert-primary game-top-message">
            You are hosting the game. Send this access code to your friend:{" "}
            <b>{this.props.gameAccessCode}</b>
            <p>
              You can setup your fleet and start game when you both connected.{" "}
            </p>
          </div>
        </div>
      );
    } else if (
      this.props.currentState === consts.GameState.COMPLETED &&
      this.props.gameType === consts.GameType.JOIN
    ) {
      message = (
        <div className="row">
          <div className="col alert alert-primary game-top-message">
            Waiting for host to start new round....
          </div>
        </div>
      );
    }
    return (
      <React.Fragment>
        <Prompt
          when={this.props.currentState == consts.GameState.STARTED}
          message="If you navigate from this page your game will end. Are you sure you want to end this game?"
        />
        {message}
        <div className="row justify-content-center">
          <div className="col">
            <GameBoard boardType={consts.BoardType.MY_BOARD} />
            <BoardStatus />
          </div>
          <div className="col">{enemyBoard}</div>
        </div>
        <div className="row">
          <div className="col">
            <GameMenu />
          </div>
        </div>
      </React.Fragment>
    );
  }
}

GamePage.propTypes = {
  currentState: PropTypes.string.isRequired,
  gameType: PropTypes.string.isRequired,
  gameAccessCode: PropTypes.string,
  connectionId: PropTypes.string,
  authenticated: PropTypes.bool.isRequired,
  isFriendConnected: PropTypes.bool.isRequired,
  actions: PropTypes.object.isRequired,
  location: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  currentState: state.gameState.currentState,
  gameType: state.gameState.gameType,
  gameAccessCode: state.gameState.multiplayer.gameAccessCode,
  connectionId: state.signalrState.connectionId,
  authenticated: state.authState.authenticated,
  isFriendConnected: state.gameState.multiplayer.isFriendConnected
});

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default withRouter(
  connect(
    mapStateToProps,
    mapDispatchToProps
  )(GamePage)
);
