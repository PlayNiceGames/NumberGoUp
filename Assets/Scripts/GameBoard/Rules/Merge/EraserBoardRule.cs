using System.Collections.Generic;
using System.Linq;
using GameBoard.Turns;
using Tiles;

namespace GameBoard.Rules.Merge
{
    public class EraserBoardRule : BoardRule
    {
        public EraserBoardRule(Board board) : base(board)
        {
        }

        public override BoardTurn GetTurn()
        {
            List<EraserTile> eraserTiles = _board.GetAllTiles<EraserTile>().ToList();

            if (eraserTiles.Count == 0)
                return null;

            return new EraserBoardTurn(_board, eraserTiles);
        }
    }
}