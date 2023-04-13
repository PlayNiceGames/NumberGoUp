using Tiles;

namespace GameBoard.Rules
{
    public class MergeBoardRule : BoardRule
    {
        public MergeBoardRule(Board board) : base(board)
        {
        }

        public override BoardTurn GetTurn(Tile tile)
        {
            if (tile is RegularTile regularTile)
                return GetRegularTileTurn(regularTile);
        }
    }
}