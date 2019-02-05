import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";
import { Link } from "react-router-dom";

class ResetPasswordStep2Page extends React.Component {
  constructor(props, context) {
    super(props, context);

    const params = new URLSearchParams(this.props.location.search);
    const code = params.get("code");

    this.state = {
      code: code,
      password: "",
      password2: "",
      message: null,
      submitted: false
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(e) {
    const { name, value } = e.target;
    this.setState({ [name]: value });
  }

  handleSubmit(e) {
    e.preventDefault();
    let message = null;

    const { code, password, password2 } = this.state;
    if (password !== password2) {
      message = "Passwords do not match";
    }

    this.setState({ message });
    if (message !== null) {
      return;
    }

    this.setState({ submitted: true });
    this.props.actions.resetPassword(code, password, password2);
  }

  render() {
    let msg = "";
    if (this.props.resetPasswordDone) {
      msg = this.props.resetPasswordSuccess ? (
        <div className="alert alert-success" role="alert">
          <i className="far fa-check-circle" /> Password reset. Please{" "}
          <Link to="login">Login</Link>
        </div>
      ) : (
        <div className="alert alert-warning" role="alert">
          {this.props.resetPasswordMessage}
        </div>
      );
    } else if (!this.state.submitted && this.state.message) {
      msg = (
        <div className="alert alert-warning" role="alert">
          {this.state.message}
        </div>
      );
    }

    return (
      <React.Fragment>
        <form className="form-signin" onSubmit={this.handleSubmit}>
          <h2 className="form-signin-heading">Change password</h2>

          <div className="form-group">
            <label htmlFor="password">New Password</label>
            <input
              type="password"
              autoComplete="new-password"
              className="form-control"
              id="password"
              name="password"
              placeholder="Password"
              required
              value={this.state.password}
              onChange={this.handleChange}
            />
          </div>
          <div className="form-group">
            <label htmlFor="password2">Repeat Password</label>
            <input
              type="password"
              autoComplete="new-password"
              className="form-control"
              id="password2"
              name="password2"
              placeholder="Repeat Password"
              required
              value={this.state.password2}
              onChange={this.handleChange}
            />
          </div>

          {msg}
          <button
            className="btn btn-lg btn-primary btn-block"
            type="submit"
            disabled={this.state.submitted}
          >
            Reset password
          </button>
        </form>
      </React.Fragment>
    );
  }
}

ResetPasswordStep2Page.propTypes = {
  resetPasswordDone: PropTypes.bool.isRequired,
  resetPasswordSuccess: PropTypes.bool,
  resetPasswordMessage: PropTypes.string,
  actions: PropTypes.object.isRequired,
  location: PropTypes.object
};

const mapStateToProps = (state, ownProps) => ({
  resetPasswordDone: state.authState.resetPasswordDone,
  resetPasswordSuccess: state.authState.resetPasswordSuccess,
  resetPasswordMessage: state.authState.resetPasswordMessage
});

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ResetPasswordStep2Page);
