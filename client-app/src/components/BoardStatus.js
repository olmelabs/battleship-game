import React from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';

class BoardStatus extends React.Component {
  constructor(props, context){
    super(props, context);
  }

  render() {
    let message = "";
    if (!this.props.isMyBoardValid){
      message = "Your board is not valid";
    }

    return(
      <div className={message === "" ? "d-none": "alert alert-warning"}>
        <strong>{message}</strong>
      </div>);
  }
}

BoardStatus.propTypes = {
  isMyBoardValid: PropTypes.bool.isRequired,
};

const mapStateToProps = (state, ownProps) => ({
  isMyBoardValid: state.gameState.isMyBoardValid
});

export default connect(
  mapStateToProps
)(BoardStatus);
