using GameLoop.GameRules;
using Tiles;

namespace TileQueue
{
    public class QueueGenerator
    {
        private TileFactory _factory;
        private Rules _rules;

        public QueueGenerator(TileFactory factory, Rules rules)
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