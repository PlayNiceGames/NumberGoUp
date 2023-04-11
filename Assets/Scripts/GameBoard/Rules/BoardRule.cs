using System.Collections.Generic;
using Tiles;

namespace GameBoard.Rules
{
    public abstract class BoardRule
    {
        public abstract List<BoardTurn> CreateActions(List<Tile> tiles);
    }
}