using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameBoard;
using GameBoard.Rules;
using GameBoard.Turns;
using GameDebug;
using GameLoop.Rules;
using GameOver;
using GameScore;
using GameTileQueue;
using Tiles;
using UnityEngine;

namespace GameLoop
{
    public class EndlessModeGameLoop : GameLoop
    {
        [SerializeField] private GameLoopSettings _settings;
        [SerializeField] private Board _board;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private GameRules _gameRules;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private GameOverUI _gameOverUI;

        [SerializeField] private DebugController _debugController;

        private UniTaskCompletionSource<Tile> _emptyTileClicked;
        private BoardRules _boardRules;
        private GameOverController _gameOver;

        private void Start()
        {
            _boardRules = new BoardRules(_board, _scoreSystem);
            _gameOver = new GameOverController(_gameOverUI, _board, _scoreSystem, _settings.GameOverSettings);

            _gameOver.Setup();
            _gameRules.Setup();
            _tileQueue.Setup();

            int initialBoardSize = _gameRules.CurrentRules.BoardSize;
            _board.Setup(initialBoardSize);
            _board.OnTileClick += OnTileClicked;

            Run().Forget();

            if (_debugController != null)
                _debugController.Setup();
        }

        public override async UniTask Run()
        {
            while (true)
            {
                await ProcessUserInput();
                await ProcessBoardRules();
                AgeTiles();
                UpdateGameRules();

                bool shouldEndGame = await _gameOver.TryProcessGameOver();

                if (shouldEndGame)
                {
                    Debug.LogError("GAME OVER");
                    return;
                }
            }
        }

        private void UpdateGameRules()
        {
            _gameRules.UpdateCurrentRules();

            int boardSize = _gameRules.CurrentRules.BoardSize;
            _board.UpdateBoardSize(boardSize);
        }

        private async UniTask ProcessBoardRules()
        {
            while (true)
            {
                BoardTurn turn = _boardRules.GetFirstAvailableTurn();

                if (turn == null)
                    return;

                await turn.Run();
                await UniTask.WaitForEndOfFrame(this);

                Debug.Log($"RAN turn {turn.GetType()}");
            }
        }

        private async Task ProcessUserInput()
        {
            Tile clickedTile = await WaitTileClicked();

            Tile newTile = GetNextTile();
            newTile.transform.position = new Vector3(clickedTile.transform.position.x, clickedTile.transform.position.y, 0); //TODO temp

            _board.PlaceTile(newTile, clickedTile.BoardPosition);
        }

        private Tile GetNextTile()
        {
            if (IsDebugPlaceTiles())
                return _debugController.GetTestTile();

            return _tileQueue.GetNextTile();
        }

        private UniTask<Tile> WaitTileClicked()
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

        private void AgeTiles()
        {
            AgeTilesBoardTurn turn = new AgeTilesBoardTurn(_board);
            turn.Run().Forget();
        }

        private bool IsDebugPlaceTiles()
        {
            return DebugController.IsDebug && _debugController.DebugPlaceTiles; //TODO rewrite debug
        }
    }
}