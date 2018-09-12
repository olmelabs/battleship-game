import React from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { bindActionCreators } from 'redux';
import * as actions from '../actions';
import { Link } from 'react-router-dom';

class ConfirmEmailPage extends React.Component {
  constructor(props, context){
    super(props, context);

    const params = new URLSearchParams(this.props.location.search);
    const code = params.get('code');

    this.props.actions.confirmEmail(code);
  }

  render() {
    const msg = this.props.emailConfirmSuccess ?
      (<span>Your email is now confirmed. Please <Link to="login">Login</Link> to your account.</span>) :
      (this.props.emailConfirmMessage);
    return(
      <div className="px-3 py-3 pt-md-5 pb-md-4 mx-auto text-center">
        <p className="lead">{msg}</p>
      </div>
      );
  }
}

ConfirmEmailPage.propTypes = {
  emailConfirmSuccess: PropTypes.bool,
  emailConfirmMessage: PropTypes.string,
  actions: PropTypes.object.isRequired,
  location: PropTypes.object
};

const mapStateToProps = (state, ownProps) => ({
  emailConfirmSuccess: state.authState.emailConfirmSuccess,
  emailConfirmMessage: state.authState.emailConfirmMessage,
});

function mapDispatchToProps(dispatch){
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ConfirmEmailPage);
