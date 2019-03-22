import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { BarLoader } from "react-spinners";
import i18n from "../../helpers/i18n";

//TODO: Fix localization
class FooterControl extends React.Component {
  constructor(props, context) {
    super(props, context);
  }

  render() {
    const year = new Date().getFullYear();
    return (
      <footer className="footer">
        <div className="container text-center">
          &copy; olmelabs 2018 - {year}.{" "}
          <a href="//github.com/olmelabs/battleship-game" target="_blank">
            {i18n.t("common.footer.devsite")}
          </a>
        </div>
        <div className="container center">
          <div className="bar-loader">
            <BarLoader color={"#123abc"} loading={this.props.isLoading} />
          </div>
        </div>
      </footer>
    );
  }
}

FooterControl.propTypes = {
  isLoading: PropTypes.bool.isRequired,
  lng: PropTypes.string
};

const mapStateToProps = (state, ownProps) => ({
  isLoading: state.ajaxState.ajaxCallIsnProgress > 0,
  lng: state.localizationState.languageCode //required to switch anf on the fly
});

export default connect(mapStateToProps)(FooterControl);
