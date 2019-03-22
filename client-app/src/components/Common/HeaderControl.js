/* eslint-disable react/jsx-no-bind */
import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { bindActionCreators } from "redux";
import * as actions from "../../actions";

export class HeaderControl extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.handleLogoutClick = this.handleLogoutClick.bind(this);
    this.setLanguageCode = this.setLanguageCode.bind(this);
  }

  handleLogoutClick(e) {
    this.props.actions.logout();
  }

  setLanguageCode(lang) {
    this.props.actions.setLanguage(lang);
    localStorage.setItem("localizationState.languageCode", lang);
  }

  render() {
    const accountLink = this.props.authenticated ? (
      <a
        href="#"
        className="p-2 text-dark"
        test-id="logout-link"
        onClick={this.handleLogoutClick}
      >
        Logout
      </a>
    ) : (
      <Link className="p-2 text-dark" test-id="login-link" to="login">
        Login
      </Link>
    );

    return (
      <div className="d-flex flex-column flex-md-row align-items-center p-3 px-md-4 mb-3 bg-white border-bottom box-shadow">
        <h5 className="my-0 mr-md-auto font-weight-normal" />
        <nav className="my-2 my-md-0 mr-md-3">
          <Link className="p-2 text-dark" to="/">
            Home
          </Link>
          Lang:{" "}
          <a href="#" onClick={() => this.setLanguageCode("ru")}>
            ru
          </a>
          {" | "}
          <a href="#" onClick={() => this.setLanguageCode("en")}>
            en
          </a>
          {/* <Link className="p-2 text-dark" to="profile">
            Profile
          </Link>
          {accountLink} */}
        </nav>
      </div>
    );
  }
}

HeaderControl.propTypes = {
  authenticated: PropTypes.bool.isRequired,
  actions: PropTypes.object.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  authenticated: state.authState.authenticated
});

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(HeaderControl);
