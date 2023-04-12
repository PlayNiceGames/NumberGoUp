using Tiles;

namespace GameBoard.Rules
{
    public abstract class BoardRule
    {
        protected Board _board;

        public BoardRule(Board board)
        {
            _board = board;
        }

        public abstract BoardTurn GetTurn(Tile tile);
    }
}