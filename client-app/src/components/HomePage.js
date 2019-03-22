import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import LocalizedStrings from "react-localization";

export class HomePage extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.strings = new LocalizedStrings({
      en: {
        welcome: "Welcome to Battleship game",
        button1: "Singleplayer Game",
        button2: "Host Game",
        button3: "Join Game"
      },
      ru: {
        welcome: "Добро пожаловать на морской бой",
        button1: "Игра с компьютером",
        button2: "Создать игру с другом",
        button3: "Подключиться к игре"
      }
    });
    this.strings.setLanguage(props.lang);
  }

  componentDidUpdate(prevProps) {
    if (prevProps.lang != this.props.lang) {
      this.strings.setLanguage(this.props.lang);
    }
  }

  render() {
    return (
      <div className="px-3 py-3 pt-md-5 pb-md-4 mx-auto text-center">
        <p className="lead">{this.strings.welcome}!</p>
        <p test-id="homepage-text">
          <Link to="game">
            <button
              className="round-button round-button-80"
              title={this.strings.button1}
            >
              <i className="fa fa-laptop fa-2x" />
            </button>
          </Link>
          <Link to="host">
            <button
              className="round-button round-button-80"
              title={this.strings.button2}
            >
              <i className="fa fa-home fa-2x" />
            </button>
          </Link>
          <Link to="code">
            <button
              className="round-button round-button-80"
              title={this.strings.button3}
            >
              <i className="fa fa-handshake fa-2x" />
            </button>
          </Link>
        </p>
        <p className="lead">
          Battleship is a guessing game. You can find out more in this{" "}
          <a href="https://en.wikipedia.org/wiki/Battleship_(game)">
            wikipedia article
          </a>
          . On this site you can have a quick game with computer or enjoy
          session with your friend. This game is intentioanlly kept simple and
          close to pen-and-paper version so popular in my childhood.
        </p>
      </div>
    );
  }
}

HomePage.propTypes = {
  lang: PropTypes.string
};

const mapStateToProps = (state, ownProps) => ({
  lang: state.localizationState.languageCode
});

export default connect(mapStateToProps)(HomePage);
