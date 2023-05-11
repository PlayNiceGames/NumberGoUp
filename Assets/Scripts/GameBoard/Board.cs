using System;
using System.Collections.Generic;
using Tiles;
using Tiles.Data;
using UnityEngine;

namespace GameBoard
{
    public class Board : MonoBehaviour
    {
        public event Action<Tile> OnTileClick;
        public int Size { get; private set; }

        [SerializeField] private BoardGrid _grid;
        [SerializeField] private TileFactory _factory;

        private Tile[,] _tiles;
        private BoardResizer _resizer;

        public void Setup(int size)
        {
            ClearBoard();

            _tiles = new Tile[size, size];
            _resizer = new BoardResizer(this, _factory);
            Size = size;

            ValidateBoard();
        }

        private void ClearBoard()
        {
            if (_tiles == null)
                return;

            foreach (Tile tile in _tiles)
                tile.Dispose();

            _tiles = null;
        }

        public Tile CreateTile(TileData data, Vector2Int position)
        {
            Tile tile = _factory.InstantiateTile(data);
            PlaceTile(tile, position);

            return tile;
        }

        public void ClearTile(Vector2Int position)
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

        public Tile GetTile(Vector2Int position)
        {
            if (!IsPositionValid(position))
                return null;

            return _tiles[position.x, position.y];
        }

        public IEnumerable<Tile> GetAllTiles()
        {
            foreach (Tile tile in _tiles)
                yield return tile;
        }

        public IEnumerable<T> GetAllTiles<T>() where T : Tile
        {
            foreach (Tile tile in _tiles)
                if (tile is T tileOfType)
                    yield return tileOfType;
        }

        private bool IsPositionValid(Vector2Int position)
        {
            return position.x >= 0 && position.y >= 0 && position.x < Size && position.y < Size;
        }

        private Vector2 GetWorldPosition(Vector2Int boardPosition)
        {
            return _tiles[boardPosition.x, boardPosition.y].transform.position;
        }

        private void UpdateGrid()
        {
            _grid.UpdateGrid(_tiles);
        }

        public IEnumerable<Tile> GetNearbyTiles(Vector2Int position)
        {
            yield return GetTile(new Vector2Int(position.x - 1, position.y));
            yield return GetTile(new Vector2Int(position.x + 1, position.y));
            yield return GetTile(new Vector2Int(position.x, position.y - 1));
            yield return GetTile(new Vector2Int(position.x, position.y + 1));
        }

        public IEnumerable<Tile> GetDiagonalTiles(Vector2Int position)
        {
            yield return GetTile(new Vector2Int(position.x - 1, position.y - 1));
            yield return GetTile(new Vector2Int(position.x - 1, position.y + 1));
            yield return GetTile(new Vector2Int(position.x + 1, position.y - 1));
            yield return GetTile(new Vector2Int(position.x + 1, position.y + 1));
        }

        public void UpdateBoardSize(int size)
        {
            if (size == Size)
                return;

            _tiles = _resizer.ResizeBoard(size, _tiles);

            Size = size;

            ValidateBoard();
        }

        private void ValidateBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Tile tile = _tiles[i, j];

                    if (tile == null)
                    {
                        EmptyTile emptyTile = _factory.InstantiateTile<EmptyTile>();
                        AddTile(emptyTile, new Vector2Int(i, j));
                    }
                    else
                    {
                        tile.BoardPosition = new Vector2Int(i, j);
                    }
                }
            }

            UpdateGrid();
        }
    }
}