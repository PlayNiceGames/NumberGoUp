using Analytics;
using Cysharp.Threading.Tasks;
using GameAnalytics.Events.Game;
using GameAudio;
using GameBoard;
using GameBoard.Actions;
using GameBoard.Rules;
using GameLoop.Rules;
using GameOver;
using GameSave;
using GameScore;
using GameSettings;
using GameTileQueue;
using GameTileQueue.Generators;
using ServiceLocator;
using Tiles;
using UnityEngine;
using Utils;

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
        [SerializeField] private GameExitButton _exitButton;
        [SerializeField] private GameDataSerializer _serializer;

        private GameSaveService _saveService;
        private Audio _audio;
        private AnalyticsService _analytics;

        private BoardRules _boardRules;
        private EndlessModeTileQueueGenerator _tileQueueGenerator;
        private GameOverController _gameOver;

        public override void Setup()
        {
            _saveService = GlobalServices.Get<GameSaveService>();
            _audio = GlobalServices.Get<Audio>();
            _analytics = GlobalServices.Get<AnalyticsService>();

            _scoreSystem.SetEnabled(true);

            _tileQueueGenerator = new EndlessModeTileQueueGenerator(_tileQueueGeneratorSettings, _gameRules, _scoreSystem);
            _tileQueue.Setup(_tileQueueGenerator);

            _gameOver = new GameOverController(_gameOverUI, _board, _scoreSystem, _settings.GameOverSettings, _tileQueue, _analytics);

            _serializer.Setup(_gameOver);
        }

        public override async UniTask RunNewGame()
        {
            _analytics.Send(new GameStartEvent(GameLoopType.EndlessMode));

            _gameRules.SetInitialData();

            int initialBoardSize = _gameRules.CurrentRules.BoardSize;
            BoardData boardData = BoardData.EmptySquare(initialBoardSize);
            UniTask setupBoardTask = SetupBoardWithAnimation(boardData);
            UniTask setupTileQueueTask = _tileQueue.SetupInitialTilesWithAnimation();

            await UniTask.WhenAll(setupBoardTask, setupTileQueueTask);

            await Run();
        }

        public override async UniTask RunSavedGame(GameData save)
        {
            _analytics.Send(new GameStartEvent(GameLoopType.EndlessMode));

            _gameRules.SetData(save.GameRulesData);
            _scoreSystem.SetData(save.Score);
            _gameOver.SetData(save.GameOverContinueCount);

            _gameRules.UpdateCurrentRules();

            UniTask setupBoardTask = SetupBoardWithAnimation(save.BoardData);
            UniTask setupTileQueueTask = _tileQueue.SetDataWithAnimation(save.TileQueueData);

            await UniTask.WhenAll(setupBoardTask, setupTileQueueTask);

            await Run();
        }

        protected override async UniTask Run()
        {
            while (true)
            {
                UniTask boardInputTask = _boardLoop.ProcessUserInput();
                UniTask backButtonClickTask = _exitButton.WaitForClick();

                await UniTask.WhenAny(boardInputTask, backButtonClickTask);

                if (backButtonClickTask.IsCompleted())
                    break;

                await _boardLoop.ProcessBoard();

                _gameRules.UpdateCurrentRules();

                await TryUpdateBoardSize();

                bool showEndGame = await _gameOver.TryProcessGameOver();

                SaveGame();

                if (showEndGame)
                {
                    _saveService.DeleteCurrentSave();
                    break;
                }
            }

            ExitGame();
        }

        private void SaveGame()
        {
            GameData data = _serializer.GetData();
            _saveService.Save(data);
        }

        private UniTask SetupBoardWithAnimation(BoardData boardData)
        {
            SetupBoardAction setupBoardAction = new SetupBoardAction(boardData, _tileFactory, _board);

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

        private void ExitGame()
        {
            GameSceneManager.LoadMainMenu();
        }
    }
}