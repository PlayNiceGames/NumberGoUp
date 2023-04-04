using GameRules;
using Tiles;

namespace GameTileQueue
{
    public class TileQueueGenerator
    {
        private TileQueueGeneratorSettings _settings;
        private TileFactory _factory;
        private Rules _rules;

        private TileQueueSet _currentSet;
        private TileQueueSet _nextSet;

        private int _bigTileNotGeneratedCount;
        private int _mixedTileNotGeneratedCount;

        public TileQueueGenerator(TileQueueGeneratorSettings settings, TileFactory factory, Rules rules)
        {
            _settings = settings;
            _factory = factory;

            SetRules(rules);

            _currentSet = new TileQueueSet()
        }

        public void SetRules(Rules rules)
        {
            _rules = rules;
        }

        private Tile[] GenerateNextQueue()
        {
            Tile[] queue = new Tile[_settings.TileQueueSize];

            return queue;
        }

        public Tile InstantiateNextTile()
        {
            RegularTile tile = _factory.InstantiateTile<RegularTile>();
            tile.SetColor(_rules.GetRandomTileColor());
            return tile;
        }
    }
}