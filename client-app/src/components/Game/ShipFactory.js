import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";
import * as consts from "../../helpers/const";
import ShipDefinition from "./ShipDefinition";
import i18n from "../../helpers/i18n";

class ShipFactory extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.onRotateShipClick = this.onRotateShipClick.bind(this);
    this.onGenerateBoardClick = this.onGenerateBoardClick.bind(this);
  }

  onRotateShipClick() {
    if (
      this.props.currentState === consts.GameState.STARTED ||
      this.props.currentState === consts.GameState.COMPLETED ||
      this.props.myShipsCurrent < 0
    ) {
      return;
    }
    this.props.actions.shipRotated({
      rotation: true
    });
  }

  onGenerateBoardClick() {
    if (
      this.props.currentState === consts.GameState.STARTED ||
      this.props.currentState === consts.GameState.COMPLETED
    ) {
      return;
    }

    this.props.actions.generateBoard();
  }

  render() {
    const isGenerateActive =
      this.props.currentState === consts.GameState.NOT_STARTED &&
      (this.props.gameType === consts.GameType.SINGLEPLAYER ||
        (this.props.gameType === consts.GameType.HOST &&
          this.props.isFriendConnected &&
          !this.props.startGameSuccess) ||
        (this.props.gameType === consts.GameType.JOIN &&
          this.props.isFriendConnected &&
          !this.props.startGameSuccess));

    return (
      <React.Fragment>
        <div className="centered text-center">
          <p>{i18n.t("game.shipFactory.setupFleet")}</p>
          {this.props.myShips.map((ship, index) => (
            <ShipDefinition
              key={index}
              index={index}
              ship={ship}
              active={isGenerateActive}
              currentShip={this.props.myShipsCurrent}
            />
          ))}
        </div>
        <div className="container">
          <div className="centered text-center">
            <button
              className="round-button round-button-80"
              title={i18n.t("game.shipFactory.rotate")}
              onClick={this.onRotateShipClick}
            >
              <i className="fa fa-redo fa-2x" />
            </button>
          </div>
        </div>
        <div className="centered text-center">
          <p>{i18n.t("game.shipFactory.generateBoard")}</p>
          <button
            disabled={!isGenerateActive}
            className="control-button btn btn-primary"
            title={i18n.t("game.shipFactory.generate")}
            onClick={this.onGenerateBoardClick}
          >
            {i18n.t("game.shipFactory.generate")}
          </button>
        </div>
      </React.Fragment>
    );
  }
}

ShipFactory.propTypes = {
  gameType: PropTypes.string.isRequired,
  isFriendConnected: PropTypes.bool.isRequired,
  startGameSuccess: PropTypes.bool.isRequired,
  myShips: PropTypes.array.isRequired,
  myShipsCurrent: PropTypes.number.isRequired,
  currentState: PropTypes.string.isRequired,
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  gameType: state.gameState.gameType,
  isFriendConnected: state.gameState.multiplayer.isFriendConnected,
  startGameSuccess: state.gameState.multiplayer.startGameSuccess,
  myShips: state.gameState.myShips,
  myShipsCurrent: state.gameState.myShipsCurrent,
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
)(ShipFactory);
