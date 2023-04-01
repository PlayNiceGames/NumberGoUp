using GameLoop.GameRules;
using Tiles;

namespace GameTileQueue
{
    public class TileQueueGenerator
    {
        private TileFactory _factory;
        private Rules _rules;

        public TileQueueGenerator(TileFactory factory, Rules rules)
        {
            _factory = factory;

            SetRules(rules);
        }

        public void SetRules(Rules rules)
        {
            _rules = rules;
        }

        public Tile InstantiateNextTile()
        {
            RegularTile tile = _factory.InstantiateTile<RegularTile>();
            tile.SetNumber(1);
            tile.SetColor(_rules.GetRandomTileColor());
            return tile;
        }
    }
}