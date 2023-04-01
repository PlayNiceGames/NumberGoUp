using Tiles;
using UnityEngine;

namespace GameBoard
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private BoardGrid _grid;
        [SerializeField] private TileFactory _factory;

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

                    SetTile(tile, new Vector2Int(i, j));
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

        public void PlaceTile(Tile tile, Vector2Int position)
        {
            ClearTile(position);

            SetTile(tile, position);

            UpdateGrid();
        }

        private void ClearTile(Vector2Int position)
        {
            Tile tile = _tiles[position.x, position.y];

            if (tile != null)
                tile.Dispose();
        }

        private void SetTile(Tile tile, Vector2Int position)
        {
            tile.BoardPosition = position;
            tile.name = position.ToString();

            _tiles[position.x, position.y] = tile;
        }

        private void UpdateGrid()
        {
            _grid.UpdateGrid(_tiles);
        }
    }
}