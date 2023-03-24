using Tile;
using UnityEngine;

namespace Board.UI
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private BoardGrid _grid;
        [SerializeField] private TileFactory _factory;

        private TileBase[,] _tiles;
        
        private void Awake()
        {
            CreateBoard(10, 10);
        }
        
        public void CreateBoard(int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    EmptyTile tile = _factory.InstantiateEmptyTile();

                    _tiles[i, j] = tile;
                }
            }
        }
    }
}