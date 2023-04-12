using System.Collections.Generic;
using Tiles;

namespace GameBoard.Rules
{
    public class BoardRulesDatabase //TODO maybe move rules to RegularTile and MixedTile?
    {
        private List<RegularTileBoardRule> _regularTileBoardRules;
        private List<MixedTileBoardRule> _mixedTileBoardRules;

        public BoardRulesDatabase()
        {
            _regularTileBoardRules = new List<RegularTileBoardRule>
            {
                new TestBoardRule()
            };

            _mixedTileBoardRules = new List<MixedTileBoardRule>();
        }

        public BoardTurn GetTurn(Tile tile)
        {
            if (tile is RegularTile regularTile)
            {
                
            }
        } 

        public List<BoardRule> GetRules(TileType type)
        {
            switch (type)
            {
                case TileType.Regular:
                    return _regularTileBoardRules;
                case TileType.Mixed:
                    return _mixedTileBoardRules;
                default:
                    return null;
            }
        }
    }
}