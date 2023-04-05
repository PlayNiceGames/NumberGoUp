﻿using System.Collections.Generic;
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

        private Queue<TileData> _generatedTileQueue;

        public TileQueueGenerator(TileQueueGeneratorSettings settings, TileFactory factory, Rules rules)
        {
            _settings = settings;
            _factory = factory;

            _generatedTileQueue = new Queue<TileData>();

            SetRules(rules);
        }

        public void SetRules(Rules rules)
        {
            _rules = rules;
        }

        public TileData GetNextTileData()
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