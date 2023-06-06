using System.Collections.Generic;
using GameLoop.Rules;
using GameScore;
using Tiles.Data;

namespace GameTileQueue.Generators
{
    public class EndlessModeTileQueueGenerator : TileQueueGenerator
    {
        private TileQueueGeneratorSettings _settings;
        private GameRules _rules;
        private ScoreSystem _scoreSystem;

        private TileQueueSet _currentSet;
        private TileQueueSet _nextSet;

        private int _bigTileNotGeneratedCount;
        private int _mixedTileNotGeneratedCount;

        private Queue<TileData> _generatedTileQueue;

        public EndlessModeTileQueueGenerator(TileQueueGeneratorSettings settings, GameRules rules, ScoreSystem scoreSystem)
        {
            _settings = settings;
            _scoreSystem = scoreSystem;
            _rules = rules;

            _generatedTileQueue = new Queue<TileData>();
        }

        public override TileData GetNextTileData()
        {
            if (_generatedTileQueue.Count == 0)
                GenerateNextSet();

            TileData tileData = _generatedTileQueue.Dequeue();
            return tileData;
        }

        private void GenerateNextSet()
        {
            _currentSet = _nextSet;
            _nextSet = new TileQueueSet(_currentSet, _settings, _rules, _scoreSystem.Score);

            TileData[] nextTileSet = _nextSet.Generate();
            foreach (TileData tileData in nextTileSet)
            {
                _generatedTileQueue.Enqueue(tileData);
            }
        }
    }
}