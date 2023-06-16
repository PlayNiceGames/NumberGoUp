using Tiles;
using UnityEngine;
using UnityEngine.UI;

namespace GameBoard
{
    public class BoardGrid : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _grid;

        public void AddTileOffGrid(Tile tile)
        {
            tile.SetIgnoreGrid(true);
            tile.SetParent(transform);
        }

        public void UpdateGrid(Tile[,] tiles)
        {
            int size = tiles.GetLength(0);
            _grid.constraintCount = size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Tile tile = tiles[i, j];

                    tile.SetIgnoreGrid(false);
                    tile.SetParent(transform);
                }
            }
        }
    }
}