using Tile;
using UnityEngine;

namespace Board
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private BoardGrid _grid;
        [SerializeField] private TileFactory _factory;

        private TileBase[,] _tiles;

        public void CreateBoard(int size)
        {
            ClearBoard();

            _tiles = new TileBase[size, size];

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

            foreach (TileBase tile in _tiles)
                tile.Dispose();

            _tiles = null;
        }

        public void PlaceTile(TileBase tile, Vector2Int position)
        {
            ClearTile(position);

            SetTile(tile, position);

            UpdateGrid();
        }

        private void ClearTile(Vector2Int position)
        {
            TileBase tile = _tiles[position.x, position.y];

            if (tile != null)
                tile.Dispose();
        }

        private void SetTile(TileBase tile, Vector2Int position)
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