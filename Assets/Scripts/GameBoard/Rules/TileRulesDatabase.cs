using System.Collections.Generic;
using Tiles;

namespace GameBoard.Rules
{
    public class TileRulesDatabase
    {
        private Dictionary<TileType, List<BoardRule>> _rules = new Dictionary<TileType, List<BoardRule>>();
    }
}