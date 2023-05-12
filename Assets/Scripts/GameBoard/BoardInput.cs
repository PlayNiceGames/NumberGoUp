using System;
using Cysharp.Threading.Tasks;
using Tiles;
using UnityEngine;

namespace GameBoard
{
    public class BoardInput : MonoBehaviour
    {
        [SerializeField] private Board _board;

        private UniTaskCompletionSource<Tile> _emptyTileClicked;

        private void Awake()
        {
            _board.OnTileClick += OnTileClicked;
        }

        private void OnDestroy()
        {
            _board.OnTileClick -= OnTileClicked;
        }

        private UniTask<Tile> WaitUntilTileClicked(TileType type)
        {
            _emptyTileClicked = new UniTaskCompletionSource<Tile>();
            return _emptyTileClicked.Task;
        }

        private void OnTileClicked(Tile tile)
        {
            if (tile.Type == TileType.Empty || IsDebugPlaceTiles())
            {
                _emptyTileClicked?.TrySetResult(tile);
            }
        }
    }
}