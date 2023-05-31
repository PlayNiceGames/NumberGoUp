﻿using Cysharp.Threading.Tasks;
using GameDebug;
using Tiles;
using UnityEngine;

namespace GameBoard
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

        public UniTask<Tile> WaitUntilTileClicked(TileType type)
        {
            _currentExpectedTileType = type;

            _emptyTileClicked = new UniTaskCompletionSource<Tile>();
            return _emptyTileClicked.Task;
        }

        public UniTask<Tile> WaitUntilTileClicked()
        {
            _currentExpectedTileType = null;

            _emptyTileClicked = new UniTaskCompletionSource<Tile>();
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