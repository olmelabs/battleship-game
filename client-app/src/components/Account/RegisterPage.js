import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";
import { Link } from "react-router-dom";

class RegisterPage extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.props.actions.registrationReset();

    this.state = {
      email: "",
      firstName: "",
      lastName: "",
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

    const { email, firstName, lastName, password, password2 } = this.state;
    if (password !== password2) {
      message = "Passwords do not match";
    }

    this.setState({ message });
    if (message !== null) {
      return;
    }
    this.props.actions.registrationReset();
    this.setState({ submitted: true });
    this.props.actions.register({ email, firstName, lastName, password });
  }

  render() {
    let msg = "";
    if (this.props.registrationDone) {
      msg = this.props.registrationSuccess ? (
        <div className="alert alert-success" role="alert">
          <i className="far fa-check-circle" /> Registration successful.
          Confirmation email sent to your account.
        </div>
      ) : (
        <div className="alert alert-warning" role="alert">
          {this.props.registrationMessage}
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
      <div className="row justify-content-center">
        <div className="col-md-8 col-lg-4 col-xs-12">
          <form onSubmit={this.handleSubmit}>
            <div className="form-group">
              <label htmlFor="email">Email Address</label>
              <input
                type="email"
                autoComplete="email"
                className="form-control"
                id="email"
                name="email"
                placeholder="Email"
                required
                value={this.state.email}
                onChange={this.handleChange}
              />
            </div>
            <div className="form-group">
              <label htmlFor="password">Password</label>
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
            <div className="form-group">
              <label htmlFor="firstName">First Name</label>
              <input
                type="text"
                autoComplete="given-name"
                className="form-control"
                id="firstName"
                name="firstName"
                placeholder="First Name"
                required
                value={this.state.firstName}
                onChange={this.handleChange}
              />
            </div>
            <div className="form-group">
              <label htmlFor="lastName">Last Name</label>
              <input
                type="text"
                autoComplete="family-name"
                className="form-control"
                id="lastName"
                name="lastName"
                placeholder="Last Name"
                required
                value={this.state.lastName}
                onChange={this.handleChange}
              />
            </div>
            {msg}
            <button
              className="btn btn-lg btn-primary btn-block"
              type="submit"
              disabled={this.state.submitted && this.props.registrationSuccess}
            >
              Register
            </button>
          </form>
        </div>
      </div>
    );
  }
}

RegisterPage.propTypes = {
  registrationDone: PropTypes.bool.isRequired,
  registrationSuccess: PropTypes.bool,
  registrationMessage: PropTypes.string,
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  registrationDone: state.authState.registrationDone,
  registrationSuccess: state.authState.registrationSuccess,
  registrationMessage: state.authState.registrationMessage
});

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(RegisterPage);
