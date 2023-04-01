using System.Collections.Generic;
using GameLoop.GameRules;
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
            _generator = new QueueGenerator(_factory, _rules.CurrentRules);

            AddInitialTiles();
        }

        public void SetRules(RulesSet rules)
        {
            _generator.SetRules(rules);
        }

        private void AddInitialTiles()
        {
            _tiles = new Queue<Tile>();

            for (int i = 0; i < TileQueueSize; i++)
            {
                AddNextTile();
            }
        }

        public void AddNextTile()
        {
            Tile tile = _generator.InstantiateNextTile();
            tile.SetParent(_grid);

            _tiles.Enqueue(tile);
        }

        private void RemoveFirstTile()
        {
            _tiles.TryDequeue(out Tile tile);

            tile.ClearParent();
        }
    }
}