import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";
import i18n from "../../helpers/i18n";

class ShipDefinition extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.onSelectShipClick = this.onSelectShipClick.bind(this);
    this.onMouseEnter = this.onMouseEnter.bind(this);
    this.onMouseLeave = this.onMouseLeave.bind(this);
  }

  onSelectShipClick() {
    this.props.actions.shipChanged({
      index: this.props.index
    });
  }

  onMouseEnter() {
    this.props.actions.highLightShip({
      index: this.props.index,
      highlight: true
    });
  }

  onMouseLeave() {
    this.props.actions.highLightShip({
      index: this.props.index,
      highlight: false
    });
  }

  render() {
    let className =
      this.props.currentShip === this.props.index
        ? "round-button round-button-64 ship-button-active"
        : "round-button round-button-64";
    if (this.props.ship.cells[0] !== null) {
      className += " ship-button-assigned";
    }
    return (
      <button
        className={className}
        disabled={!this.props.active}
        title={i18n.t("game.shipDefinition.selectShip")}
        onClick={this.onSelectShipClick}
        onMouseEnter={this.onMouseEnter}
        onMouseLeave={this.onMouseLeave}
      >
        {this.props.ship.title}
      </button>
    );
  }
}

ShipDefinition.propTypes = {
  ship: PropTypes.object.isRequired,
  active: PropTypes.bool.isRequired,
  currentShip: PropTypes.number.isRequired,
  index: PropTypes.number.isRequired,
  lng: PropTypes.string,
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  lng: state.localizationState.languageCode //required to switch anf on the fly
});

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ShipDefinition);
