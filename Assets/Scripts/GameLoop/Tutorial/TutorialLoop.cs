using Cysharp.Threading.Tasks;
using GameTileQueue;
using Tutorial;
using Tutorial.Data;
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

        public override void Setup()
        {
            _tileQueue.Setup();
            _tileQueue.AddInitialTiles();

            _boardLoop.Setup();

            _dialogUI.Setup();
            _dialogUI.Show();
        }

        public override async UniTask Run()
        {
            foreach (ITutorialStepData stepData in _data.Steps)
            {
                TutorialStep step = _stepFactory.CreateStep(stepData);
                await step.Run();
            }
        }
    }
}