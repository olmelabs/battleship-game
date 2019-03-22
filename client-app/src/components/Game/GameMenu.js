import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import GameLink from "./GameLink";
import * as consts from "../../helpers/const";
import i18n from "../../helpers/i18n";

class GameMenu extends React.Component {
  render() {
    const isStartActive =
      this.props.currentState === consts.GameState.NOT_STARTED &&
      (this.props.gameType === consts.GameType.SINGLEPLAYER ||
        (this.props.gameType === consts.GameType.HOST &&
          this.props.isFriendConnected &&
          !this.props.startGameSuccess) ||
        (this.props.gameType === consts.GameType.JOIN &&
          this.props.isFriendConnected &&
          !this.props.startGameSuccess));

    const isRestartVisible =
      this.props.gameType === consts.GameType.HOST &&
      this.props.currentState === consts.GameState.COMPLETED;
    const isRestartActive = isRestartVisible;

    const isStopVisible = this.props.gameType === consts.GameType.SINGLEPLAYER;
    const isStopActive = this.props.currentState === consts.GameState.STARTED;

    const isNewVisible = this.props.gameType === consts.GameType.SINGLEPLAYER;
    const isNewActive = this.props.currentState === consts.GameState.COMPLETED;

    return (
      <div className="game-menu">
        <GameLink newAction={consts.GameState.STARTED} isActive={isStartActive}>
          {i18n.t("game.gameMenu.start")}
        </GameLink>
        {isRestartVisible && (
          <GameLink newAction={consts.RESTART_GAME} isActive={isRestartActive}>
            {i18n.t("game.gameMenu.playAgain")}
          </GameLink>
        )}
        {isStopVisible && (
          <GameLink
            newAction={consts.GameState.COMPLETED}
            isActive={isStopActive}
          >
            {i18n.t("game.gameMenu.finish")}
          </GameLink>
        )}
        {isNewVisible && (
          <GameLink
            newAction={consts.GameState.NOT_STARTED}
            isActive={isNewActive}
          >
            {i18n.t("game.gameMenu.newGame")}
          </GameLink>
        )}
      </div>
    );
  }
}

GameMenu.propTypes = {
  gameType: PropTypes.string.isRequired,
  isFriendConnected: PropTypes.bool.isRequired,
  startGameSuccess: PropTypes.bool.isRequired,
  currentState: PropTypes.string.isRequired
};

const mapStateToProps = (state, ownProps) => ({
  gameType: state.gameState.gameType,
  isFriendConnected: state.gameState.multiplayer.isFriendConnected,
  startGameSuccess: state.gameState.multiplayer.startGameSuccess,
  currentState: state.gameState.currentState
});

export default connect(mapStateToProps)(GameMenu);
