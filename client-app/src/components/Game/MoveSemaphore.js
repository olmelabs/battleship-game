import React, { useState } from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";

import "./MoveSemaphore.css";

function MoveSemaphore(props) {
  const redClassName = props.isServerTurn
    ? "circle-base semaphore-red"
    : "circle-base";

  const yellowClassName =
    props.myBoardLocked && !props.isServerTurn
      ? "circle-base semaphore-yellow"
      : "circle-base";

  const greenClassName =
    !props.myBoardLocked && !props.isServerTurn
      ? "circle-base semaphore-green"
      : "circle-base";

  return (
    <div className="semaphore-container">
      <div className={redClassName} title="Not your move" />
      <div className={yellowClassName} title="Move in progress" />
      <div className={greenClassName} title="Your move" />
    </div>
  );
}

MoveSemaphore.propTypes = {
  myBoardLocked: PropTypes.bool.isRequired,
  isServerTurn: PropTypes.bool.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  myBoardLocked: state.gameState.myBoardLocked,
  isServerTurn: state.gameState.isServerTurn
});

export default connect(mapStateToProps)(MoveSemaphore);
