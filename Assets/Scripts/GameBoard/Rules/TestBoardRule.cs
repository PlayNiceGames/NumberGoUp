using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace GameBoard.Rules
{
    public class TestBoardRule : RegularTileBoardRule
    {
        public TestBoardRule()
        {
        }

        public override List<BoardTurn> CreateActions(Tile mainTile, List<Tile> tiles)
        {
            if (_tile.Value == 2)
                Debug.Log("2 TILE!!!");

            return null;
        }
    }
}