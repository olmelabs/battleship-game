/* eslint-disable react/jsx-no-bind */
import React, { useState } from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";
import { Redirect } from "react-router";
import i18n from "../../helpers/i18n";

//this is an example of component immplementation using hooks instead of React.Component
function JoinGamePage(props) {
  const [code, setCode] = useState("");

  const handleSubmit = e => {
    e.preventDefault();
    props.actions.joinGame(code);
  };

  if (props.gameAccessCode != null) {
    return <Redirect to="/join" />;
  }
  return (
    <React.Fragment>
      <div className="row">
        <div className="col alert alert-primary game-top-message">
          {i18n.t("game.joinGamePage.message")}
        </div>
      </div>
      <div className="row justify-content-center">
        <div className="col-md-8 col-lg-4 col-xs-12">
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <input
                type="text"
                className="form-control"
                id="txtCode"
                placeholder={i18n.t("game.joinGamePage.accessCode")}
                onChange={e => setCode(e.target.value)}
              />
            </div>
            {props.joinGameError}
            {props.joinGameError && (
              <div className="alert alert-warning" role="alert">
                {i18n.t("game.joinGamePage.errorJoining")}
              </div>
            )}
            <button
              className="btn btn-lg btn-primary btn-block"
              type="submit"
              disabled={code === ""}
            >
              {i18n.t("game.joinGamePage.join")}
            </button>
          </form>
        </div>
      </div>
    </React.Fragment>
  );
}

JoinGamePage.propTypes = {
  joinGameError: PropTypes.bool.isRequired,
  gameAccessCode: PropTypes.string,
  lng: PropTypes.string,
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  joinGameError: state.gameState.multiplayer.joinGameError,
  gameAccessCode: state.gameState.multiplayer.gameAccessCode,
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
)(JoinGamePage);
