import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import GameBoard from "./GameBoard";
import BoardStatus from "./BoardStatus";
import GameMenu from "./GameMenu";
import ShipFactory from "./ShipFactory";
import * as actions from "../../actions";
import * as consts from "../../helpers/const";
import toastr from "toastr";
import { withRouter } from "react-router";

class GamePage extends React.Component {
  constructor(props, context) {
    super(props, context);
  }

  componentDidMount() {
    const { location } = this.props;

    if (location.pathname === "/host") {
      this.props.actions.initGameType(consts.GameType.HOST);
    } else if (location.pathname === "/join") {
      this.props.actions.initGameType(consts.GameType.JOIN);
    } else {
      this.props.actions.initGameType(consts.GameType.SINGLEPLAYER);
    }
  }

  componentWillReceiveProps(nextProps) {
    if (nextProps.currentState == consts.GameState.COMPLETED) {
      toastr.success("Game Over");
    }
  }

  render() {
    const enemyBoard =
      this.props.currentState === consts.GameState.STARTED ||
      this.props.currentState === consts.GameState.COMPLETED ? (
        <GameBoard boardType={consts.BoardType.ENEMY_BOARD} />
      ) : (
        <ShipFactory />
      );

    return (
      <React.Fragment>
        <div>
          {this.props.gameType} {this.props.gameAccessCode}
        </div>
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
  actions: PropTypes.object.isRequired,
  location: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  currentState: state.gameState.currentState,
  gameType: state.gameState.gameType,
  gameAccessCode: state.gameState.gameAccessCode
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
