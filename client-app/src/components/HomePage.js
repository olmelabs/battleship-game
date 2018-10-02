import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';

export class HomePage extends React.Component {
  constructor(props, context){
    super(props, context);
  }

  render() {
    const accountLink = this.props.authenticated ?
    (""):
    (<p><Link to="login">Login</Link> or <Link to="register">Register</Link> to get the best experience</p>);

    return(
      <div className="px-3 py-3 pt-md-5 pb-md-4 mx-auto text-center">
        {/* <h1 className="display-4">Welcome</h1> */}
        <p className="lead">This is battle-ship game to demo React, Redux, SignalR, .Net Core API, Docker and many other cool pieces of technology.</p>
        {accountLink}
      </div>
      );
  }
}

HomePage.propTypes = {
  authenticated: PropTypes.bool.isRequired,
};

const mapStateToProps = (state, ownProps) => ({
  authenticated: state.authState.authenticated,
});

export default connect(
  mapStateToProps
)(HomePage);
