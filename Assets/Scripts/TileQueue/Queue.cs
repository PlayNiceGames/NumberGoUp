using System.Collections.Generic;
using GameLoop.GameRules;
using Sirenix.OdinInspector;
using Tiles;
using UnityEngine;

namespace TileQueue
{
    public class Queue : MonoBehaviour
    {
        public const int TileQueueSize = 4;

        [SerializeField] private TileFactory _factory;
        [SerializeField] private Transform _grid;
        [SerializeField] private Rules _rules;

        private QueueGenerator _generator;
        private Queue<Tile> _tiles;

        public void Setup()
        {
            _generator = new QueueGenerator(_factory, _rules);
            _tiles = new Queue<Tile>();

            AddInitialTiles();
        }

        [Button]
        private void AddInitialTiles()
        {
            ClearTiles();

            for (int i = 0; i < TileQueueSize; i++)
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

        public void AddNextTile()
        {
            Tile tile = _generator.InstantiateNextTile();
            tile.SetParent(_grid);

            _tiles.Enqueue(tile);
        }

        private bool RemoveFirstTile()
        {
            bool result = _tiles.TryDequeue(out Tile tile);

            tile.ClearParent();

            return result;
        }
    }
}