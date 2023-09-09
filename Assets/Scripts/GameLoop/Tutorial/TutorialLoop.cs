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
            return RunGame();
        }

        public override UniTask RunSavedGame(GameData save)
        {
            return RunGame();
        }

        private async UniTask RunGame()
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
                _analytics.Send(new TutorialStepEvent(stepData.Name, index));

                TutorialStep step = _stepFactory.CreateStep(stepData);

                bool shouldEndTutorial = await step.Run();

                if (shouldEndTutorial)
                    break;

                index++;
            }

            EndTutorial();
        }

        private void EndTutorial()
        {
            _analytics.Send(new TutorialEndEvent());

            GameSceneManager.LoadEndlessMode();
        }
    }
}