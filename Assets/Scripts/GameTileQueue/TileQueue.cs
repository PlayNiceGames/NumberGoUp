using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tiles;
using Tiles.Data;
using UnityEngine;

namespace GameTileQueue
{
    public class TileQueue : MonoBehaviour
    {
        private const int QueueSize = 4; //TODO temp, move

        [SerializeField] private Transform _grid;
        [SerializeField] private TileFactory _factory;

        private TileQueueGenerator _generator;
        private Queue<Tile> _tiles;

        public void Setup(TileQueueGenerator generator)
        {
            _generator = generator;
            _tiles = new Queue<Tile>();
        }

        public Tile PopNextTile()
        {
            Tile firstTile = RemoveFirstTile();

            AddNextTile();

            return firstTile;
        }

        public Tile PeekNextTile()
        {
            return _tiles.TryPeek(out Tile result) ? result : null;
        }

        [Button]
        public void AddInitialTiles()
        {
            ClearTiles();

            for (int i = 0; i < QueueSize; i++)
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