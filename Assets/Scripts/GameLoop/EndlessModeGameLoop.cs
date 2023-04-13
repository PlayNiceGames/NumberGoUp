using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameBoard;
using GameBoard.Rules;
using GameLoop.Rules;
using GameTileQueue;
using Tiles;
using UnityEngine;

namespace GameLoop
{
    public class EndlessModeGameLoop : GameLoop
    {
        [SerializeField] private Board _board;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private GameRules _gameRules;
        [SerializeField] private BoardRules _boardRules;

        private UniTaskCompletionSource<Tile> _emptyTileClicked;

        private void Start()
        {
            _gameRules.Setup();
            _tileQueue.Setup();

            _board.Setup(7);
            _board.OnTileClick += OnTileClicked;

            _boardRules = new BoardRules(_board);

            Run().Forget();
        }

        public override async UniTask Run()
        {
            while (true)
            {
                await ProcessRules();
                await ProcessUserInput();
            }
        }

        private async UniTask ProcessRules()
        {
            while (true)
            {
                BoardTurn turn = _boardRules.GetFirstAvailableTurn();

                if (turn == null)
                    return;

                await turn.Run();
            }
        }

        private async Task ProcessUserInput()
        {
            Tile clickedTile = await WaitTileClicked();

            Tile newTile = _tileQueue.PlaceTileOnBoard(clickedTile.transform.position);
            _board.PlaceTile(newTile, clickedTile.BoardPosition);
        }

        private UniTask<Tile> WaitTileClicked()
        {
            _emptyTileClicked = new UniTaskCompletionSource<Tile>();
            return _emptyTileClicked.Task;
        }

        private void OnTileClicked(Tile tile)
        {
            if (tile.Type == TileType.Empty)
            {
                _emptyTileClicked?.TrySetResult(tile);
            }
        }
    }
}