import React from "react";
import { connect } from "react-redux";
import { Link, Redirect } from "react-router-dom";
import PropTypes from "prop-types";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";

class LoginPage extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      email: "",
      password: "",
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

    this.setState({ submitted: true });
    const { email, password } = this.state;
    if (email && password) {
      this.props.actions.login(email, password);
    }
  }

  render() {
    const { from } = this.props.location.state || { from: { pathname: "/" } };
    if (this.props.authenticated) {
      return <Redirect to={from} />;
    }

    const warning = this.props.loginFailed ? (
      <div className="alert alert-warning" role="alert">
        Login failed
      </div>
    ) : (
      ""
    );
    return (
      <React.Fragment>
        <form className="form-signin" onSubmit={this.handleSubmit}>
          <h2 className="form-signin-heading">Please sign in</h2>

          <label htmlFor="email" className="sr-only">
            Email address
          </label>
          <input
            type="email"
            autoComplete="email"
            id="email"
            name="email"
            className="form-control form-top-input"
            placeholder="Email address"
            value={this.state.email}
            required
            autoFocus
            onChange={this.handleChange}
          />

          <label htmlFor="password" className="sr-only">
            Password
          </label>
          <input
            type="password"
            autoComplete="current-password"
            id="password"
            name="password"
            className="form-control form-bottom-input"
            placeholder="Password"
            value={this.state.password}
            required
            onChange={this.handleChange}
          />

          {warning}
          <button className="btn btn-lg btn-primary btn-block" type="submit">
            Sign in
          </button>
          <p>
            <Link to="register">Register</Link> -{" "}
            <Link to="password_reset_link">Forgot Password?</Link>
          </p>
        </form>
      </React.Fragment>
    );
  }
}

LoginPage.propTypes = {
  authenticated: PropTypes.bool.isRequired,
  loginFailed: PropTypes.bool.isRequired,
  actions: PropTypes.object.isRequired,
  location: PropTypes.object
};

const mapStateToProps = (state, ownProps) => ({
  authenticated: state.authState.authenticated,
  loginFailed: state.authState.loginFailed
});

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(LoginPage);
