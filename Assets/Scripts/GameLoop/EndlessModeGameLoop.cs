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
    public class EndlessModeGameLoop : AbstractGameLoop
    {
        [SerializeField] private GameLoopSettings _settings;
        [SerializeField] private Board _board;
        [SerializeField] private BoardInput _boardInput;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private GameRules _gameRules;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private GameOverUI _gameOverUI;

        [SerializeField] private DebugController _debugController;

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

            Run().Forget();

            if (_debugController != null)
                _debugController.Setup();
        }

        public override async UniTask Run()
        {
            while (true)
            {
                await ProcessUserInput();
                await _boardRules.ProcessRules();
                AgeTiles();
                _gameRules.UpdateCurrentRules();
                UpdateBoardSize();

                bool shouldEndGame = await _gameOver.TryProcessGameOver();

                if (shouldEndGame)
                {
                    Debug.LogError("GAME OVER");
                    return;
                }
            }
        }

        private void UpdateBoardSize()
        {
            int boardSize = _gameRules.CurrentRules.BoardSize;
            _board.UpdateBoardSize(boardSize);
        }

        private async Task ProcessUserInput()
        {
            Tile clickedTile = await _boardInput.WaitUntilTileClicked(TileType.Empty);

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