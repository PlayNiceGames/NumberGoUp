using System.Collections.Generic;
using Tiles.Data;
using UnityEngine;

namespace GameTileQueue
{
    public class TutorialTileQueueGenerator : TileQueueGenerator
    {
        private readonly TileData _defaultTile;
        private readonly Dictionary<int, TileData> _tileOverrides;

        private int _tileIndex = 0;

        public TutorialTileQueueGenerator(TileData defaultTile, Dictionary<int, TileData> tileOverrides)
        {
            _defaultTile = defaultTile;
            _tileOverrides = tileOverrides;
        }

        public override TileData GetNextTileData()
        {
            TileData tile = _tileOverrides.TryGetValue(_tileIndex, out TileData overrideTile) ? overrideTile : _defaultTile;

            _tileIndex++;

            Debug.Log($"[Tutorial] Current tile index: {_tileIndex}");

            return tile;
        }
    }
}