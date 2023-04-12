using Tiles;
using UnityEngine;

namespace GameBoard.Rules
{
    public class TestBoardRule : BoardRule
    {
        public TestBoardRule(Board board) : base(board)
        {
        }

        public override BoardTurn GetTurn(Tile tile)
        {
            if (tile is not RegularTile regularTile)
                return null;

            if (regularTile.Value == 2)
                Debug.Log("2 TILE!!!");

            return null;
        }
    }
}