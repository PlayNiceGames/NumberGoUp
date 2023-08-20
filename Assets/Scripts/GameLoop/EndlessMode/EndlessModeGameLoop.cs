using Analytics;
using Cysharp.Threading.Tasks;
using GameAnalytics.Events;
using GameAnalytics.Events.Game;
using GameAudio;
using GameBoard;
using GameBoard.Actions;
using GameBoard.Rules;
using GameLoop.Rules;
using GameOver;
using GameScore;
using GameTileQueue;
using GameTileQueue.Generators;
using ServiceLocator;
using Tiles;
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
        [SerializeField] private TileFactory _tileFactory;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private GameOverUI _gameOverUI;

        private Audio _audio;
        private AnalyticsService _analytics;

        private BoardRules _boardRules;
        private EndlessModeTileQueueGenerator _tileQueueGenerator;
        private GameOverController _gameOver;

        public override void Setup()
        {
            _audio = GlobalServices.Get<Audio>();
            _analytics = GlobalServices.Get<AnalyticsService>();
            
            _scoreSystem.SetEnabled(true);

            _gameRules.Setup();

            _tileQueueGenerator = new EndlessModeTileQueueGenerator(_tileQueueGeneratorSettings, _gameRules, _scoreSystem);
            _tileQueue.Setup(_tileQueueGenerator);

            _boardLoop.Setup();

            _gameOver = new GameOverController(_gameOverUI, _board, _scoreSystem, _settings.GameOverSettings, _analytics);

            _gameOver.Setup();
        }

        public override async UniTask Run()
        {
            _analytics.Send(new GameStartEvent(GameLoopType.EndlessMode));

            await SetupScene();

            while (true)
            {
                await _boardLoop.Run();

                _gameRules.UpdateCurrentRules();

                await TryUpdateBoardSize();

                bool shouldEndGame = await _gameOver.TryProcessGameOver();

                if (shouldEndGame)
                    break;
            }

            EndGame();
        }

        private UniTask SetupScene()
        {
            int initialBoardSize = _gameRules.CurrentRules.BoardSize;
            UniTask setupBoardTask = SetupBoard(initialBoardSize);

            UniTask setupTileQueueTask = _tileQueue.AddInitialTilesWithAnimation();

            return UniTask.WhenAll(setupBoardTask, setupTileQueueTask);
        }

        private UniTask SetupBoard(int size)
        {
            BoardData initialBoardData = BoardData.Square(size);
            SetupBoardAction setupBoardAction = new SetupBoardAction(initialBoardData, _tileFactory, _board);

            return setupBoardAction.Run();
        }

        private UniTask TryUpdateBoardSize()
        {
            int newSize = _gameRules.CurrentRules.BoardSize;

            if (_board.Size == newSize)
                return UniTask.CompletedTask;

            ResizeBoardAction resizeAction = new ResizeBoardAction(newSize, _tileFactory, _board, _audio);

            return resizeAction.Run();
        }

        private void EndGame()
        {
            GameSceneManager.LoadMainMenu();
        }
    }
}