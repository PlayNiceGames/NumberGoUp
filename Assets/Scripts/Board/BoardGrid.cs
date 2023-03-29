using Tile;
using UnityEngine;
using UnityEngine.UI;

namespace Board
{
    public class BoardGrid : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _grid;

        public void UpdateGrid(TileBase[,] tiles)
        {
            ClearGrid();

            int size = tiles.GetLength(0);
            _grid.constraintCount = size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    TileBase tile = tiles[i, j];
                    tile.transform.SetParent(transform, false);
                }
            }
        }

        private void ClearGrid()
        {
            transform.DetachChildren();
        }
    }
}