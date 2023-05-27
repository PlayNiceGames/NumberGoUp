using Cysharp.Threading.Tasks;
using GameLoop;
using Tutorial.Steps.Data;

namespace Tutorial.Steps
{
    public class BoardTurnTutorialStep : TutorialStep
    {
        private BoardTurnTutorialStepData _data;

        private BoardGameLoop _loop;

        public BoardTurnTutorialStep(BoardTurnTutorialStepData data, BoardGameLoop loop)
        {
            _data = data;
            _loop = loop;
        }

        public override async UniTask<bool> Run()
        {
            for (int i = 0; i < _data.TurnCount; i++)
            {
                await _loop.Run();
            }

            return false;
        }
    }
}