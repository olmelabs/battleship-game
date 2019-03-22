import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import i18n from "../../helpers/i18n";

class BoardStatus extends React.Component {
  constructor(props, context) {
    super(props, context);
  }

  render() {
    let message = "";
    if (!this.props.isMyBoardValid) {
      message = i18n.t("game.boardStatus.message");
    }

    return (
      <div className={message === "" ? "d-none" : "alert alert-warning"}>
        <strong>{message}</strong>
      </div>
    );
  }
}

BoardStatus.propTypes = {
  isMyBoardValid: PropTypes.bool.isRequired,
  lng: PropTypes.string
};

const mapStateToProps = (state, ownProps) => ({
  isMyBoardValid: state.gameState.isMyBoardValid,
  lng: state.localizationState.languageCode //required to switch anf on the fly
});

export default connect(mapStateToProps)(BoardStatus);
