import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";
import * as consts from "../../helpers/const";

class GameBoardMyCell extends React.Component {
  constructor(props, context) {
    super(props, context);
    this.onCellClick = this.onCellClick.bind(this);
  }

  onCellClick() {
    if (
      this.props.currentState === consts.GameState.STARTED ||
      this.props.currentState === consts.GameState.COMPLETED
    ) {
      return;
    }
    if (this.props.myShipsCurrent >= 0 && !this.props.startGameSuccess) {
      this.props.actions.shipMoved({
        cellId: this.props.cellId
      });
    }
  }

  render() {
    let highLightCurrentShip = false;
    let selectCurrentShip = false;
    if (
      this.props.currentState == consts.GameState.NOT_STARTED &&
      this.props.isShipOnIt
    ) {
      if (this.props.myShipsCurrent >= 0) {
        this.props.myShips[this.props.myShipsCurrent].cells.map(cell => {
          if (cell === this.props.cellId) {
            selectCurrentShip = true;
          }
        });
      }
      if (this.props.myShipsHighlighted >= 0) {
        this.props.myShips[this.props.myShipsHighlighted].cells.map(cell => {
          if (cell === this.props.cellId) {
            highLightCurrentShip = true;
          }
        });
      }
    }

    let className = this.props.isShipOnIt ? " cell-selected" : " cell";
    if (selectCurrentShip) {
      className += " cell-highlight";
    } else if (highLightCurrentShip) {
      className += " cell-light-highlight";
    }

    if (this.props.isShotFired) {
      className += this.props.isHit ? " cell-fired-hit" : " cell-fired-miss";
    }

    return (
      <button className={className} onClick={this.onCellClick}>
        {this.props.isShotFired ? (this.props.isHit ? "X" : "-") : ""}
      </button>
    );
  }
}

GameBoardMyCell.propTypes = {
  cellId: PropTypes.number.isRequired,
  isShipOnIt: PropTypes.bool.isRequired,
  currentState: PropTypes.string.isRequired,
  isShotFired: PropTypes.bool.isRequired,
  isHit: PropTypes.bool.isRequired,
  myShips: PropTypes.array.isRequired,
  myShipsCurrent: PropTypes.number.isRequired,
  myShipsHighlighted: PropTypes.number.isRequired,
  startGameSuccess: PropTypes.bool.isRequired,
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  currentState: state.gameState.currentState,
  startGameSuccess: state.gameState.multiplayer.startGameSuccess,
  isShipOnIt:
    state.gameState.myBoard[ownProps.cellId] === 1 ||
    state.gameState.myBoard[ownProps.cellId] === 3,
  isShotFired:
    state.gameState.myBoard[ownProps.cellId] === 2 ||
    state.gameState.myBoard[ownProps.cellId] === 3,
  isHit: state.gameState.myBoard[ownProps.cellId] === 3,
  myShips: state.gameState.myShips,
  myShipsCurrent: state.gameState.myShipsCurrent,
  myShipsHighlighted: state.gameState.myShipsHighlighted
});

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(GameBoardMyCell);
