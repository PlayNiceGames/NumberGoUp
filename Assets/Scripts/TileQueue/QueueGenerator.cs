using GameLoop.GameRules;
using Tiles;

namespace TileQueue
{
    public class QueueGenerator
    {
        private TileFactory _factory;
        private RulesSet _rules;
        
        public QueueGenerator(TileFactory factory, RulesSet rules)
        {
            _factory = factory;
            
            SetRules(rules);
        }

        public void SetRules(RulesSet rules)
        {
            _rules = rules;
        }

        public Tile InstantiateNextTile()
        {
            RegularTile tile = _factory.InstantiateTile<RegularTile>();
            tile.SetNumber(1);
            tile.SetColor(_rules.RegularTileRules.GetNextTileColor());
            return tile;
        }
    }
}