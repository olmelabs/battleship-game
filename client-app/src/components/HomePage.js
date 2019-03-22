import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import i18n from "../helpers/i18n";

export class HomePage extends React.Component {
  constructor(props, context) {
    super(props, context);
  }

  render() {
    return (
      <div className="px-3 py-3 pt-md-5 pb-md-4 mx-auto text-center">
        <p className="lead">{i18n.t("homepage.welcome")}!</p>
        <p test-id="homepage-text">
          <Link to="game">
            <button
              className="round-button round-button-80"
              title={i18n.t("homepage.button1")}
            >
              <i className="fa fa-laptop fa-2x" />
            </button>
          </Link>
          <Link to="host">
            <button
              className="round-button round-button-80"
              title={i18n.t("homepage.button2")}
            >
              <i className="fa fa-home fa-2x" />
            </button>
          </Link>
          <Link to="code">
            <button
              className="round-button round-button-80"
              title={i18n.t("homepage.button3")}
            >
              <i className="fa fa-handshake fa-2x" />
            </button>
          </Link>
        </p>
        <p className="lead">
          {i18n.t("homepage.description.part1")}
          <a target="_blank" href={i18n.t("homepage.description.link.href")}>
            {i18n.t("homepage.description.link.text")}
          </a>
          {". "}
          {i18n.t("homepage.description.part2")}
        </p>
      </div>
    );
  }
}

export default connect()(HomePage);
