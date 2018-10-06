import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../actions';
import * as consts from '../../helpers/const';
import {ClipLoader} from 'react-spinners';

class GameBoardEnemyCell extends React.Component {
  constructor(props, context) {
    super(props, context);
    this.onCellClick = this.onCellClick.bind(this);
    this.state = {
      loading:false
    };
  }

  onCellClick(){
    if (this.props.currentState === consts.GameState.NOT_STARTED || this.props.currentState === consts.GameState.COMPLETED){
      return;
    }

    if (this.props.isServerTurn){
      return;
    }

    if (this.props.isShotFired || this.state.loading == true){
      return;
    }

    this.setState({loading:true});
    this.props.actions.fireCannon(this.props.cellId)
      .then(() => this.setState({loading: false}))
      .catch(error => {
        this.setState({loading: false});
      });
  }

  render() {
    return (
      <button
        className={this.props.drawBorder ? "cell-selected" : "cell"}
        onClick={this.onCellClick}>
          {this.props.isShotFired ? (this.props.isShipOnIt ? "X" : "-") : ""}
           <ClipLoader color={'#868e96'} loading={this.state.loading}  />
      </button>
    );
  }
}

GameBoardEnemyCell.propTypes = {
  cellId: PropTypes.number.isRequired,
  isShotFired: PropTypes.bool.isRequired,
  isShipOnIt: PropTypes.bool.isRequired,
  drawBorder: PropTypes.bool.isRequired,
  currentState: PropTypes.string.isRequired,
  isServerTurn: PropTypes.bool.isRequired,
  actions: PropTypes.object.isRequired
};

// 0 - shot not fired.
// 1 - shot fired - no ship
// 2 - shot fired - hit ship
// 10 - shot not fired, but when game ended there was ship on it. so draw border
// 11 - should not happen so far.
// 12 - shot fired and ship destroyed. draw borders around cells in destroyed shi[]

const mapStateToProps = (state, ownProps) => ({
  isShotFired: state.gameState.enemyBoard[ownProps.cellId] % 10 !== 0,
  isShipOnIt: state.gameState.enemyBoard[ownProps.cellId] % 10 === 2,
  drawBorder: state.gameState.enemyBoard[ownProps.cellId] >= 10,
  currentState: state.gameState.currentState,
  isServerTurn: state.gameState.isServerTurn,
});

function mapDispatchToProps(dispatch){
    return {
      actions: bindActionCreators(actions, dispatch)
    };
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(GameBoardEnemyCell);

