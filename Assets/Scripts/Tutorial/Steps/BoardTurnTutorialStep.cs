using Cysharp.Threading.Tasks;
using GameLoop;
using Tutorial.Data;

namespace Tutorial
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

        public override async UniTask Run()
        {
            for (int i = 0; i < _data.TurnCount; i++)
            {
                await _loop.Run();
            }
        }
    }
}