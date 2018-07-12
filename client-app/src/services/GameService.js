export function updateShip(oldShip, data){
  const cell = data.cell === undefined ? oldShip.cells[0] : data.cell;
  const vertical = data.vertical === undefined ? oldShip.vertical : data.vertical;
  const cells = new Array(oldShip.cells.length);
  cells[0] = cell;
  for(let i = 1; i < cells.length; i++){
    cells[i] = cell + (vertical ? 10 : 1) * i;
  }
  return Object.assign({}, oldShip, {cells, vertical});
}

export function placeShipOnBoard(board, oldShip, newShip, otherShips){
  //erase old ship
  for(let i = 0; i < oldShip.cells.length; i++){
    let resetCell = true;
    //check other ship on this cell while rearranging them
    otherShips.map(ship => {
        if (ship.cells.includes(oldShip.cells[i])){
          resetCell = false;
        }
    });
    if (resetCell){
      board[oldShip.cells[i]] = null;
    }
  }
  //put new ship
  for(let i = 0; i < newShip.cells.length; i++){
    board[newShip.cells[i]] = 1;
  }
}
