using Tile;
using UnityEngine;

namespace Board
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private BoardGrid _grid;
        [SerializeField] private TileFactory _factory;

        private TileBase[,] _tiles;

        private void Awake()
        {
            CreateBoard(7);
        }

        public void CreateBoard(int size)
        {
            ClearBoard();

            _tiles = new TileBase[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    RegularTile tile = _factory.InstantiateTile<RegularTile>();

                    PlaceTile(tile, new Vector2Int(i, j));
                }
            }

            _grid.UpdateGrid(_tiles);
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
            tile.BoardPosition = position;
            tile.name = position.ToString();

            _tiles[position.x, position.y] = tile;
        }
    }
}