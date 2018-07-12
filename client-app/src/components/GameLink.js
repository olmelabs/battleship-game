import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../actions';
import * as consts from '../helpers/const';

class GameLink extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.onButtonClick = this.onButtonClick.bind(this);
  }

  onButtonClick(){
    switch(this.props.newState)
    {
      case consts.GameState.NOT_STARTED:
        this.props.actions.setGameState(consts.GameState.NOT_STARTED, null);
        break;
      case consts.GameState.STARTED:
        this.props.actions.startNewGame();
        break;
      case consts.GameState.COMPLETED:
        this.props.actions.stopGame();
        break;
      default:
        break;
    }
  }

  render() {
    return (
      <button className={"control-button btn " + (this.props.isActive ? "btn-primary" : "btn-secondary")}
        onClick={this.onButtonClick}
        disabled={!this.props.isActive}>
          {this.props.children}
      </button>
    );
  }
}

GameLink.propTypes = {
  isActive: PropTypes.bool.isRequired,
  children: PropTypes.node.isRequired,
  newState: PropTypes.string.isRequired,
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
});

function mapDispatchToProps(dispatch){
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(GameLink);

