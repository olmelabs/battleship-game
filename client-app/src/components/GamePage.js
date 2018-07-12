import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GameBoard from './GameBoard';
import BoardStatus from './BoardStatus';
import GameMenu from './GameMenu';
import ShipFactory from './ShipFactory';
import * as actions from '../actions';
import * as consts from '../helpers/const';
import toastr from 'toastr';

class GamePage extends React.Component {
  constructor(props, context){
    super(props, context);
  }

  componentWillReceiveProps(nextProps){
    if (nextProps.currentState == consts.GameState.COMPLETED){
      toastr.success('Game Over');
    }
  }

  render(){
    const enemyBoard = (this.props.currentState === consts.GameState.STARTED || this.props.currentState === consts.GameState.COMPLETED) ?
    (<GameBoard boardType={consts.BoardType.ENEMY_BOARD}/>) : <ShipFactory />;

    return(
        <React.Fragment>
          <div className="row justify-content-center">
              <div className="col">
                <GameBoard boardType={consts.BoardType.MY_BOARD}/>
                <BoardStatus />
              </div>
              <div className="col">
                {enemyBoard}
              </div>
          </div>
          <div className="row">
              <div className="col">
                <GameMenu/>
              </div>
          </div>
      </React.Fragment>
    );
  }
}

GamePage.propTypes = {
  currentState: PropTypes.string.isRequired,
  actions: PropTypes.object.isRequired
};


const mapStateToProps = (state, ownProps) => ({
  currentState: state.gameState.currentState
});

function mapDispatchToProps(dispatch){
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(GamePage);
