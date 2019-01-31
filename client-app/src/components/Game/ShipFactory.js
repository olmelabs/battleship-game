import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";
import * as consts from "../../helpers/const";
import ShipDefinition from "./ShipDefinition";

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
    return (
      <React.Fragment>
        <div className="centered text-center">
          <p>
            Setup your fleet. Click on round buttons to setup ships. Click on
            cell on the board to place ship.
          </p>
          {this.props.myShips.map((ship, index) => (
            <ShipDefinition
              key={index}
              index={index}
              ship={ship}
              currentShip={this.props.myShipsCurrent}
            />
          ))}
        </div>
        <div className="container">
          <div className="centered text-center">
            <button
              className="round-button round-button-80"
              title="Rotate Ship"
              onClick={this.onRotateShipClick}
            >
              <i className="fa fa-redo fa-2x" />
            </button>
          </div>
        </div>
        <div className="centered text-center">
          <p>Or click the button below to quickly generate board.</p>
          <button
            className="control-button btn btn-primary"
            title="Generate"
            onClick={this.onGenerateBoardClick}
          >
            Generate
          </button>
        </div>
      </React.Fragment>
    );
  }
}

ShipFactory.propTypes = {
  myShips: PropTypes.array.isRequired,
  myShipsCurrent: PropTypes.number.isRequired,
  currentState: PropTypes.string.isRequired,
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
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
