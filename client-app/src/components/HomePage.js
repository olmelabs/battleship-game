import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import PropTypes from "prop-types";

export class HomePage extends React.Component {
  constructor(props, context) {
    super(props, context);
  }

  render() {
    return (
      <div className="px-3 py-3 pt-md-5 pb-md-4 mx-auto text-center">
        <p className="lead">Welcome to Battleship game.</p>
        <p test-id="homepage-text">
          <Link to="game">
            <button
              className="round-button round-button-80"
              title="Sigleplayer Game"
            >
              <i className="fa fa-laptop fa-2x" />
            </button>
          </Link>
          <Link to="host">
            <button className="round-button round-button-80" title="Host Game">
              <i className="fa fa-home fa-2x" />
            </button>
          </Link>
          <Link to="code">
            <button className="round-button round-button-80" title="Join Game">
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

export default connect()(HomePage);
