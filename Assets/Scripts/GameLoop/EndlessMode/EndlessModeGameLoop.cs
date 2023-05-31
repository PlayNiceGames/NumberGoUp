using Cysharp.Threading.Tasks;
using GameBoard;
using GameBoard.Rules;
using GameLoop.Rules;
using GameOver;
using GameScore;
using GameTileQueue;
using UnityEngine;

namespace GameLoop.EndlessMode
{
    public class EndlessModeGameLoop : AbstractGameLoop
    {
        [SerializeField] private GameLoopSettings _settings;
        [SerializeField] private TileQueueGeneratorSettings _tileQueueGeneratorSettings;
        [SerializeField] private GameRules _gameRules;
        [SerializeField] private BoardGameLoop _boardLoop;
        [SerializeField] private Board _board;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private GameOverUI _gameOverUI;

        private BoardRules _boardRules;
        private EndlessModeTileQueueGenerator _tileQueueGenerator;
        private GameOverController _gameOver;

        public override void Setup()
        {
            _gameRules.Setup();

            _tileQueueGenerator = new EndlessModeTileQueueGenerator(_tileQueueGeneratorSettings, _gameRules);
            _tileQueue.Setup(_tileQueueGenerator);

            int initialBoardSize = _gameRules.CurrentRules.BoardSize;
            _board.Setup(initialBoardSize);

            _boardLoop.Setup();

            _gameOver = new GameOverController(_gameOverUI, _board, _scoreSystem, _settings.GameOverSettings);

            _gameOver.Setup();
        }

        public override async UniTask Run()
        {
            _tileQueue.AddInitialTiles();

            while (true)
            {
                await _boardLoop.Run();
                _gameRules.UpdateCurrentRules();
                UpdateBoardSize();

                bool shouldEndGame = await _gameOver.TryProcessGameOver();

                if (shouldEndGame)
                    break;
            }

            EndGame();
        }

        private void UpdateBoardSize()
        {
            int newSize = _gameRules.CurrentRules.BoardSize;
            _board.UpdateBoardSize(newSize);
        }

        private void EndGame()
        {
            GameSceneManager.LoadMainMenu();
        }
    }
}