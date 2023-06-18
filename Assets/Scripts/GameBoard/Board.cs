using System;
using System.Collections.Generic;
using Serialization;
using Tiles;
using Tiles.Data;
using UnityEngine;

namespace GameBoard
{
    public class Board : MonoBehaviour, IDataSerializable<BoardData>
    {
        public event Action<Tile> OnTileClick;
        public int Size { get; private set; }

        [field: SerializeField] public BoardGrid Grid;

        [SerializeField] private TileFactory _factory;

        private Tile[,] _tiles;
        private BoardResizer _resizer;

        public void Awake()
        {
            _resizer = new BoardResizer(this);
        }

        public void SetupBoard(int size)
        {
            ClearBoard();

            Size = size;

            _tiles = new Tile[size, size];

            ValidateBoard();
        }

        public void UpdateBoardSize(int size)
        {
            if (size == Size)
                return;

            _tiles = _resizer.ResizeBoard(size, _tiles);

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

        public Tile FreeTile(Tile tile)
        {
            Vector2Int position = tile.BoardPosition;
            Tile boardTile = _tiles[position.x, position.y];

            if (tile != boardTile)
                return tile;

            tile.ClearParent();
            tile.OnClick -= OnTileClicked;

            Tile emptyTile = _factory.InstantiateTile<EmptyTile>();
            AddTile(emptyTile, position);

            UpdateGrid();

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

        public Vector2 GetWorldPosition(Vector2Int boardPosition)
        {
            return _tiles[boardPosition.x, boardPosition.y].transform.position;
        }

        private void UpdateGrid()
        {
            Grid.UpdateGrid(_tiles);
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

        private void ValidateBoard()
        {
            int sizeX = _tiles.GetLength(0);
            Size = sizeX;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Tile tile = _tiles[i, j];

                    if (tile == null)
                    {
                        VoidTile voidTile = _factory.InstantiateTile<VoidTile>();
                        AddTile(voidTile, new Vector2Int(i, j));
                    }
                    else
                    {
                        tile.BoardPosition = new Vector2Int(i, j);
                    }
                }
            }

            UpdateGrid();
        }

        public BoardData GetData()
        {
            TileData[,] tilesData = GetTilesData();
            return new BoardData(tilesData);
        }

        private TileData[,] GetTilesData()
        {
            TileData[,] tilesData = new TileData[_tiles.GetLength(0), _tiles.GetLength(1)];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Tile tile = _tiles[i, j];

                    if (tile == null)
                        continue;

                    TileData tileData = tile.GetData();
                    tilesData[i, j] = tileData;
                }
            }

            return tilesData;
        }

        public void SetData(BoardData data)
        {
            ClearBoard();

            TileData[,] tilesData = data.Tiles;
            SetTilesData(tilesData);

            ValidateBoard();
        }

        private void SetTilesData(TileData[,] tilesData)
        {
            int sizeX = tilesData.GetLength(0);
            int sizeY = tilesData.GetLength(1);

            _tiles = new Tile[sizeX, sizeY];
            Size = sizeX;

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    TileData tileData = tilesData[i, j];

                    if (tileData == null)
                        continue;

                    Tile tile = _factory.InstantiateTile(tileData);

                    AddTile(tile, new Vector2Int(i, j));
                }
            }
        }
    }
}