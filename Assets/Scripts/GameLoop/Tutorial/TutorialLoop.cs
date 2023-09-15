using Analytics;
using Cysharp.Threading.Tasks;
using GameAnalytics.Events.Tutorial;
using GameSave;
using GameScore;
using GameTileQueue;
using GameTileQueue.Generators;
using ServiceLocator;
using Tutorial;
using Tutorial.Steps;
using Tutorial.Steps.Data;
using UnityEngine;

namespace GameLoop.Tutorial
{
    public class TutorialLoop : AbstractGameLoop
    {
        [SerializeField] private TutorialDatabase _data;
        [SerializeField] private TutorialStepFactory _stepFactory;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private TileQueue _tileQueue;

        private TutorialTileQueueGenerator _tileQueueGenerator;
        private AnalyticsService _analytics;

        public override void Setup()
        {
            _scoreSystem.SetEnabled(false);

            _tileQueueGenerator = new TutorialTileQueueGenerator(_data.DefaultTileInQueue, _data.TileQueueOverrides);
            _tileQueue.Setup(_tileQueueGenerator);

            _analytics = GlobalServices.Get<AnalyticsService>();
        }

        public override UniTask RunNewGame()
        {
            return RunTutorial();
        }

        public override UniTask RunSavedGame(GameData save)
        {
            return RunTutorial();
        }

        private async UniTask RunTutorial()
        {
            _analytics.Send(new TutorialStartEvent());

            await _tileQueue.SetupInitialTilesWithAnimation();

            await Run();
        }

        protected override async UniTask Run()
        {
            int index = 0;
            foreach (TutorialStepData stepData in _data.Steps)
            {
                TutorialStep step = _stepFactory.CreateStep(stepData);

                TutorialStepResult result = await step.Run();

                _analytics.Send(new TutorialStepEvent(stepData.Name, index, result));

                if (result == TutorialStepResult.StartGame)
                {
                    StartGame();
                    break;
                }

                if (result == TutorialStepResult.ExitToMenu)
                {
                    ExitToMainMenu();
                    break;
                }

                index++;
            }
        }

        private void StartGame()
        {
            _analytics.Send(new TutorialEndEvent());

            GameSceneManager.LoadEndlessMode();
        }

        private void ExitToMainMenu()
        {
            GameSceneManager.LoadMainMenu();
        }
    }
}