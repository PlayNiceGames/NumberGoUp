using System;
using GameTileQueue;
using Tiles;
using UnityEngine;

namespace GameBoard
{
    public class Board : MonoBehaviour
    {
        public event Action<Tile> OnTileClick;

        [SerializeField] private BoardGrid _grid;
        [SerializeField] private TileFactory _factory;
        [SerializeField] private TileQueue _queue;

        private Tile[,] _tiles;

        public void Setup(int size)
        {
            ClearBoard();

            _tiles = new Tile[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    EmptyTile tile = _factory.InstantiateTile<EmptyTile>();

                    AddTile(tile, new Vector2Int(i, j));
                }
            }

            UpdateGrid();
        }

        private void ClearBoard()
        {
            if (_tiles == null)
                return;

            foreach (Tile tile in _tiles)
                tile.Dispose();

            _tiles = null;
        }

        public void PlaceEmptyTile(Vector2Int position)
        {
            Tile tile = _tiles[position.x, position.y];
            if (tile.Type == TileType.Empty)
                return;

            Tile emptyTile = _factory.InstantiateTile<EmptyTile>();
            PlaceTile(emptyTile, position);
        }

        public void PlaceTile(Tile tile, Vector2Int position)
        {
            DestroyTile(position);

            AddTile(tile, position);

            UpdateGrid();
        }

        private void DestroyTile(Vector2Int position)
        {
            Tile oldTile = _tiles[position.x, position.y];
            if (oldTile == null)
                return;

            oldTile.OnClick -= OnTileClicked;
            oldTile.Dispose();
        }

        private void AddTile(Tile tile, Vector2Int position)
        {
            tile.BoardPosition = position;
            tile.name = position.ToString();
            tile.OnClick += OnTileClicked;

            _tiles[position.x, position.y] = tile;
        }

        private void OnTileClicked(Tile tile)
        {
            OnTileClick?.Invoke(tile);
        }

        private Vector2 GetWorldPosition(Vector2Int boardPosition)
        {
            return _tiles[boardPosition.x, boardPosition.y].transform.position;
        }

        private void UpdateGrid()
        {
            _grid.UpdateGrid(_tiles);
        }
    }
}