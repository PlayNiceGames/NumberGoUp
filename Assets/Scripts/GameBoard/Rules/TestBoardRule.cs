using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace GameBoard.Rules
{
    public class TestBoardRule : RegularTileBoardRule
    {
        public TestBoardRule(RegularTile tile) : base(tile)
        {
        }

        public override List<BoardTurn> CreateActions(List<Tile> tiles)
        {
            if (_tile.Value == 2)
                Debug.Log("1 TILE!!!");

            return null;
        }
    }
}