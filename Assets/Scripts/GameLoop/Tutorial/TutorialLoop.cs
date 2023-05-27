using Cysharp.Threading.Tasks;
using GameTileQueue;
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
        [SerializeField] private BoardGameLoop _boardLoop;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private TutorialDialogUI _dialogUI;

        private TutorialTileQueueGenerator _tileQueueGenerator;

        public override void Setup()
        {
            _tileQueueGenerator = new TutorialTileQueueGenerator(_data.DefaultTileInQueue);
            _tileQueue.Setup(_tileQueueGenerator);

            _boardLoop.Setup();

            _dialogUI.Setup();
            _dialogUI.Show();
        }

        public override async UniTask Run()
        {
            _tileQueue.AddInitialTiles();

            foreach (ITutorialStepData stepData in _data.Steps)
            {
                TutorialStep step = _stepFactory.CreateStep(stepData);
                await step.Run();
            }
        }
    }
}