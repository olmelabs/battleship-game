import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import GameLink from './GameLink';
import * as consts from '../../helpers/const';

class GameMenu extends React.Component {

  render(){

    const isStartActive = this.props.currentState === consts.GameState.NOT_STARTED;
    const isStopActive =  this.props.currentState === consts.GameState.STARTED;
    const isNewActive = this.props.currentState === consts.GameState.COMPLETED;

    return (
      <div className="game-menu">
        {/* <div>Current State: {this.props.currentState}</div> */}
        {/* <span>Actions: </span> */}
        <GameLink newState={consts.GameState.STARTED} isActive={isStartActive}>
            Start
        </GameLink>
        <GameLink newState={consts.GameState.COMPLETED} isActive={isStopActive}>
            Finish
        </GameLink>
        <GameLink newState={consts.GameState.NOT_STARTED} isActive={isNewActive}>
            New Game
        </GameLink>
      </div>
    );
  }
}

GameMenu.propTypes = {
  currentState: PropTypes.string.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  currentState: state.gameState.currentState,
});

export default connect(
  mapStateToProps
)(GameMenu);

