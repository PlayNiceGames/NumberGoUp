﻿using System.Collections.Generic;
using GameLoop.Rules;
using Tiles.Data;

namespace GameTileQueue
{
    public class EndlessModeTileQueueGenerator : TileQueueGenerator
    {
        private TileQueueGeneratorSettings _settings;
        private GameRules _rules;

        private TileQueueSet _currentSet;
        private TileQueueSet _nextSet;

        private int _bigTileNotGeneratedCount;
        private int _mixedTileNotGeneratedCount;

        private Queue<TileData> _generatedTileQueue;

        public EndlessModeTileQueueGenerator(TileQueueGeneratorSettings settings, GameRules rules)
        {
            _settings = settings;

            _generatedTileQueue = new Queue<TileData>();

            SetRules(rules);
        }

        public void SetRules(GameRules rules)
        {
            _rules = rules;
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
            _nextSet = new TileQueueSet(_currentSet, _settings, _rules);

            TileData[] nextTileSet = _nextSet.Generate();
            foreach (TileData tileData in nextTileSet)
            {
                _generatedTileQueue.Enqueue(tileData);
            }
        }
    }
}