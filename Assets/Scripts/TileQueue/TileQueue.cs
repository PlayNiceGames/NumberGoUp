using System.Collections.Generic;
using GameRules;
using Sirenix.OdinInspector;
using Tiles;
using UnityEngine;

namespace GameTileQueue
{
    public class TileQueue : MonoBehaviour
    {
        [SerializeField] private Transform _grid;
        [SerializeField] private TileQueueGeneratorSettings _settings;
        [SerializeField] private TileFactory _factory;
        [SerializeField] private Rules _rules;

        private TileQueueGenerator _generator;
        private Queue<Tile> _tiles;

        public void Setup()
        {
            _generator = new TileQueueGenerator(_settings, _factory, _rules);
            _tiles = new Queue<Tile>();

            AddInitialTiles();
        }

        public Tile PlaceTileOnBoard(Vector3 tileBoardWorldPosition)
        {
            Tile firstTile = RemoveFirstTile();
            firstTile.transform.position = new Vector3(tileBoardWorldPosition.x, tileBoardWorldPosition.y, 0); //TODO temp

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
            Tile tile = _generator.InstantiateNextTile();
            tile.SetParent(_grid);

            _tiles.Enqueue(tile);
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