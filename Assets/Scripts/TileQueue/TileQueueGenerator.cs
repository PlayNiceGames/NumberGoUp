using GameLoop.GameRules;
using Tiles;

namespace GameTileQueue
{
    public class TileQueueGenerator
    {
        private TileFactory _factory;
        private Rules _rules;

        private Tile[] _currentQueue;
        private int _mixedTileNotGeneratedCount;

        public TileQueueGenerator(TileFactory factory, Rules rules)
        {
            _factory = factory;

            SetRules(rules);
        }

        public void SetRules(Rules rules)
        {
            _rules = rules;
        }

        private Tile[] GenerateNextQueue()
        {
            Tile[] queue = new Tile[TileQueue.TileQueueSize];

            return queue;
        }

        public Tile InstantiateNextTile()
        {
            RegularTile tile = _factory.InstantiateTile<RegularTile>();
            tile.SetColor(_rules.GetRandomTileColor());
            return tile;
        }

        private RegularTile GenerateRegularTile()
        {
            return null;
        }

        private MixedTile GenerateMixedTile()
        {
            return null;
        }
    }
}