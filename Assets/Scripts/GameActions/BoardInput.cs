using System.Threading;
using Cysharp.Threading.Tasks;
using GameBoard;
using GameDebug;
using Tiles;
using UnityEngine;
using Utils;

namespace GameActions
{
    public class BoardInput : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private DebugTilePlacer _debugTilePlacer;

        private UniTaskCompletionSource<Tile> _emptyTileClicked;
        private TileType? _currentExpectedTileType;

        private void Awake()
        {
            _board.OnTileClick += OnTileClicked;
        }

        private void OnDestroy()
        {
            _board.OnTileClick -= OnTileClicked;
        }

        public UniTask<Tile> WaitUntilTileClicked(TileType type, CancellationToken cancellationToken)
        {
            _currentExpectedTileType = type;

            _emptyTileClicked = new UniTaskCompletionSource<Tile>();
            _emptyTileClicked.AttachCancellationToken(cancellationToken);

            return _emptyTileClicked.Task;
        }

        public UniTask<Tile> WaitUntilTileClicked(CancellationToken cancellationToken)
        {
            _currentExpectedTileType = null;

            _emptyTileClicked = new UniTaskCompletionSource<Tile>();
            _emptyTileClicked.AttachCancellationToken(cancellationToken);

            return _emptyTileClicked.Task;
        }

        private void OnTileClicked(Tile tile)
        {
            if (_currentExpectedTileType == null || tile.Type == _currentExpectedTileType || IsDebugPlaceTiles())
            {
                _emptyTileClicked?.TrySetResult(tile);
            }
        }

        private bool IsDebugPlaceTiles()
        {
            return DebugController.IsDebug && _debugTilePlacer.DebugPlaceTiles; //TODO rewrite debug
        }
    }
}