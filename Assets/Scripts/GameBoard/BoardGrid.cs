using Cysharp.Threading.Tasks;
using Tiles;
using UnityEngine;
using UnityEngine.UI;

namespace GameBoard
{
    public class BoardGrid : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private RectTransform _root;

        public void AddTileOffGrid(Tile tile)
        {
            tile.SetIgnoreGrid(true);
            tile.SetParent(transform);
        }

        public void MoveTileOnTop(Tile tile)
        {
            tile.transform.SetAsLastSibling();
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

                    int index = i * size + j;

                    tile.SetIgnoreGrid(false);
                    tile.SetParent(transform);
                    tile.transform.SetSiblingIndex(index);
                }
            }
        }

        public async UniTask WaitForGridUpdate()
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        }
    }
}