using System.Collections.Generic;
using GameBoard.Actions;
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

            if (regularTile.Value != 1)
                return null;

            Debug.Log("RULE FOR 2 TILE");

            List<BoardAction> actions = new List<BoardAction>();

            actions.Add(new TestBoardAction(regularTile));

            return new BoardTurn(actions);
        }
    }
}