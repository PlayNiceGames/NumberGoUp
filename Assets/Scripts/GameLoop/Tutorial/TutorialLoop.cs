using Cysharp.Threading.Tasks;
using GameBoard;
using GameTileQueue;
using GameTileQueue.Generators;
using Tutorial;
using Tutorial.Dialog;
using Tutorial.Steps;
using Tutorial.Steps.Data;
using UnityEngine;

namespace GameLoop.Tutorial
{
    public class TutorialLoop : AbstractGameLoop
    {
        [SerializeField] private TutorialData _data;
        [SerializeField] private TutorialStepFactory _stepFactory;
        [SerializeField] private Board _board;
        [SerializeField] private BoardGameLoop _boardLoop;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private TutorialDialogController _dialogController;

        private TutorialTileQueueGenerator _tileQueueGenerator;

        public override void Setup()
        {
            _tileQueueGenerator = new TutorialTileQueueGenerator(_data.DefaultTileInQueue, _data.TileQueueOverrides);
            _tileQueue.Setup(_tileQueueGenerator);

            _boardLoop.Setup();

            _dialogController.Setup();
        }

        public override async UniTask Run()
        {
            _tileQueue.AddInitialTiles();

            foreach (ITutorialStepData stepData in _data.Steps)
            {
                TutorialStep step = _stepFactory.CreateStep(stepData);
                bool shouldEndTutorial = await step.Run();

                if (shouldEndTutorial)
                    break;
            }

            EndTutorial();
        }

        private void EndTutorial()
        {
            GameSceneManager.LoadEndlessMode();
        }
    }
}