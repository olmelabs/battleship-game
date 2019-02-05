import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";

class ResetPasswordStep1Page extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      email: "",
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

    const { email } = this.state;
    if (email && !this.state.submitted) {
      this.props.actions.sendResetPasswordLink(email);
    }
    this.setState({ submitted: true });
  }

  render() {
    const warning = this.state.submitted ? (
      <div className="alert alert-info" role="alert">
        Email with password reset link sent to your email address.
      </div>
    ) : (
      ""
    );
    return (
      <React.Fragment>
        <form className="form-signin" onSubmit={this.handleSubmit}>
          <h2 className="form-signin-heading">Provide your email</h2>

          <label htmlFor="email" className="sr-only">
            Email address
          </label>
          <input
            type="email"
            autoComplete="email"
            id="email"
            name="email"
            className="form-control form-single-input"
            placeholder="Email address"
            value={this.state.email}
            required
            autoFocus
            onChange={this.handleChange}
          />

          {warning}
          <button className="btn btn-lg btn-primary btn-block" type="submit">
            Send Reset Link
          </button>
        </form>
      </React.Fragment>
    );
  }
}

ResetPasswordStep1Page.propTypes = {
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({});

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ResetPasswordStep1Page);
