import React from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { BarLoader } from 'react-spinners';

class FooterControl extends React.Component {
  constructor(props, context){
    super(props, context);
  }

  render() {
    return(
      <footer className="footer">
        <div className="container text-center">&copy; olmelabs 2018. <a href="//github.com/olmelabs/battleship-game" target="_blank" >Project dev site</a></div>
        <div className="container center"><div className="bar-loader"><BarLoader color={'#123abc'} loading={this.props.isLoading} /></div></div>
      </footer>
      );
  }
}

FooterControl.propTypes = {
  isLoading: PropTypes.bool.isRequired,
};

const mapStateToProps = (state, ownProps) => ({
  isLoading: state.ajaxState.ajaxCallIsnProgress > 0,
});

export default connect(
  mapStateToProps
)(FooterControl);
