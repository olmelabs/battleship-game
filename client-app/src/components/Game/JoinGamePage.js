/* eslint-disable react/jsx-no-bind */
import React, { useState } from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";
import { Redirect } from "react-router";

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
          You are joining the game. You freind who is hosting the game you are
          about to join is supposed to send you game access code. Please input
          it in the textbox below.
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
                placeholder="Access Code"
                onChange={e => setCode(e.target.value)}
              />
            </div>
            {props.joinGameError}
            {props.joinGameError && (
              <div className="alert alert-warning" role="alert">
                Error Joining Game. Check your code
              </div>
            )}
            <button
              className="btn btn-lg btn-primary btn-block"
              type="submit"
              disabled={code === ""}
            >
              Join
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
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  joinGameError: state.gameState.multiplayer.joinGameError,
  gameAccessCode: state.gameState.multiplayer.gameAccessCode
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
