import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import PropTypes from "prop-types";

export class HomePage extends React.Component {
  constructor(props, context) {
    super(props, context);
  }

  render() {
    const accountLink = this.props.authenticated ? (
      <p test-id="authorized-text">
        <Link to="game">
          <button className="round-button" title="Quick Game">
            <i className="fa fa-laptop fa-2x" />
          </button>
        </Link>
        <button className="round-button" title="Host Game">
          <i className="fa fa-home fa-2x" />
        </button>
        <button className="round-button" title="Join Game">
          <i className="fa fa-handshake fa-2x" />
        </button>
      </p>
    ) : (
      <p test-id="unauthorized-text">
        <Link to="login">Login</Link> or <Link to="register">Register</Link> to
        play with friends.
        <div>
          <Link to="game">
            <button className="round-button" title="Quick Game">
              <i className="fa fa-laptop fa-2x" />
            </button>
          </Link>
        </div>
      </p>
    );

    return (
      <div className="px-3 py-3 pt-md-5 pb-md-4 mx-auto text-center">
        <p className="lead">Welcome to battle-ship game.</p>
        {accountLink}
      </div>
    );
  }
}

HomePage.propTypes = {
  authenticated: PropTypes.bool.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  authenticated: state.authState.authenticated
});

export default connect(mapStateToProps)(HomePage);
