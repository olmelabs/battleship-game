import React from "react";
import PropTypes from "prop-types";
import GameBoardMyCell from "./GameBoardMyCell";
import GameBoardEnemyCell from "./GameBoardEnemyCell";
import * as consts from "../../helpers/const";

class GameBoard extends React.Component {
  constructor(props, context) {
    super(props, context);
  }

  renderCell(i) {
    const cell =
      this.props.boardType === consts.BoardType.MY_BOARD ? (
        <GameBoardMyCell cellId={i} />
      ) : (
        <GameBoardEnemyCell cellId={i} />
      );

    return <React.Fragment>{cell}</React.Fragment>;
  }

  renderRow(i) {
    return (
      <div className="board-row">
        {this.renderCell(i * 10)}
        {this.renderCell(i * 10 + 1)}
        {this.renderCell(i * 10 + 2)}
        {this.renderCell(i * 10 + 3)}
        {this.renderCell(i * 10 + 4)}
        {this.renderCell(i * 10 + 5)}
        {this.renderCell(i * 10 + 6)}
        {this.renderCell(i * 10 + 7)}
        {this.renderCell(i * 10 + 8)}
        {this.renderCell(i * 10 + 9)}
      </div>
    );
  }

  render() {
    return (
      <div className="game-board">
        {this.renderRow(0)}
        {this.renderRow(1)}
        {this.renderRow(2)}
        {this.renderRow(3)}
        {this.renderRow(4)}
        {this.renderRow(5)}
        {this.renderRow(6)}
        {this.renderRow(7)}
        {this.renderRow(8)}
        {this.renderRow(9)}
      </div>
    );
  }
}

GameBoard.propTypes = {
  boardType: PropTypes.string.isRequired
};

export default GameBoard;
