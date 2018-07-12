import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../actions';

class ShipDefinition extends React.Component {
  constructor(props, context){
    super(props, context);

    this.onSelectShipClick = this.onSelectShipClick.bind(this);
  }

  onSelectShipClick(){
    this.props.actions.shipChanged({
      index: this.props.index,
    });
  }

  render() {
    let className =  this.props.currentShip === this.props.index? "round-button ship-button-active" : "round-button";
    if (this.props.ship.cells[0] !== null){
      className  += " ship-button-assigned";
    }
    return (
        <button className={className} title="Select Ship" onClick={this.onSelectShipClick}>{this.props.ship.title}</button>
    );
  }
}

ShipDefinition.propTypes = {
  ship: PropTypes.object.isRequired,
  currentShip: PropTypes.number.isRequired,
  index: PropTypes.number.isRequired,
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
)(ShipDefinition);

