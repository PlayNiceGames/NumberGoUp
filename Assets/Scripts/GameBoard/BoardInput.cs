using Cysharp.Threading.Tasks;
using GameDebug;
using Tiles;
using UnityEngine;

namespace GameBoard
{
    public class BoardInput : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private DebugController _debugController;

        private UniTaskCompletionSource<Tile> _emptyTileClicked;
        private TileType _currentExpectedTileType;

        private void Awake()
        {
            _board.OnTileClick += OnTileClicked;
        }

        private void OnDestroy()
        {
            _board.OnTileClick -= OnTileClicked;
        }

        public UniTask<Tile> WaitUntilTileClicked(TileType type)
        {
            _currentExpectedTileType = type;

            _emptyTileClicked = new UniTaskCompletionSource<Tile>();
            return _emptyTileClicked.Task;
        }

        private void OnTileClicked(Tile tile)
        {
            if (tile.Type == _currentExpectedTileType || IsDebugPlaceTiles())
            {
                _emptyTileClicked?.TrySetResult(tile);
            }
        }

        private bool IsDebugPlaceTiles()
        {
            return DebugController.IsDebug && _debugController.DebugPlaceTiles; //TODO rewrite debug
        }
    }
}