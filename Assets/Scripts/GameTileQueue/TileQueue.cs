using System.Collections.Generic;
using GameLoop.Rules;
using Sirenix.OdinInspector;
using Tiles;
using Tiles.Data;
using UnityEngine;

namespace GameTileQueue
{
    public class TileQueue : MonoBehaviour
    {
        [SerializeField] private Transform _grid;
        [SerializeField] private TileQueueGeneratorSettings _settings;
        [SerializeField] private TileFactory _factory;
        [SerializeField] private GameRules _rules;

        private TileQueueGenerator _generator;
        private Queue<Tile> _tiles;

        public void Setup()
        {
            _generator = new TileQueueGenerator(_settings, _factory, _rules);
            _tiles = new Queue<Tile>();

            AddInitialTiles();
        }

        public Tile GetNextTile()
        {
            Tile firstTile = RemoveFirstTile();

            AddNextTile();

            return firstTile;
        }

        [Button]
        private void AddInitialTiles()
        {
            ClearTiles();

            for (int i = 0; i < _settings.TileQueueSize; i++)
            {
                AddNextTile();
            }
        }

        private void ClearTiles()
        {
            foreach (Tile tile in _tiles)
                tile.ClearParent();

            _tiles.Clear();
        }

        [Button]
        private void AddNextTile()
        {
            Tile tile = InstantiateNextTile();
            tile.SetParent(_grid);

            _tiles.Enqueue(tile);
        }

        private Tile InstantiateNextTile()
        {
            TileData tileData = _generator.GetNextTileData();

            Tile tile = _factory.InstantiateTile(tileData);
            return tile;
        }

        [Button]
        private Tile RemoveFirstTile()
        {
            bool result = _tiles.TryDequeue(out Tile tile);

            if (!result)
                return null;

            tile.ClearParent();

            return tile;
        }
    }
}