using System.Collections.Generic;
using GameLoop.Rules;
using GameScore;
using Tiles.Data;

namespace GameTileQueue.Generators
{
    public class EndlessModeTileQueueGenerator : TileQueueGenerator
    {
        private readonly TileQueueGeneratorSettings _settings;
        private readonly GameRules _rules;
        private readonly ScoreSystem _scoreSystem;

        private Queue<TileData> _generatedTileQueue;

        private TileQueueSet _prevSet;
        private TileQueueSet _currentSet;

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
            _prevSet = _currentSet;
            _currentSet = new TileQueueSet(_prevSet, _settings, _rules, _scoreSystem);

            TileData[] nextTileSet = _currentSet.Generate();
            foreach (TileData tileData in nextTileSet)
                _generatedTileQueue.Enqueue(tileData);
        }

        public override TileQueueGeneratorData GetData()
        {
            return new TileQueueGeneratorData(_generatedTileQueue.ToArray(), _currentSet.GetData());
        }

        public override void SetData(TileQueueGeneratorData data)
        {
            _generatedTileQueue = new Queue<TileData>(data.GeneratedTileData);

            _currentSet = new TileQueueSet(null, _settings, _rules, _scoreSystem);
            _currentSet.SetData(data.CurrentSetData);
        }
    }
}