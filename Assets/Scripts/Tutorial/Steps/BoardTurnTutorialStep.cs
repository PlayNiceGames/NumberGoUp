using Cysharp.Threading.Tasks;
using GameLoop;
using GameSettings;
using Tutorial.Steps.Data;
using Utils;

namespace Tutorial.Steps
{
    public class BoardTurnTutorialStep : TutorialStep
    {
        private readonly BoardTurnTutorialStepData _data;

        private readonly BoardGameLoop _loop;
        private readonly GameExitButton _exitButton;

        public BoardTurnTutorialStep(BoardTurnTutorialStepData data, BoardGameLoop loop, GameExitButton exitButton)
        {
            _data = data;
            _loop = loop;
            _exitButton = exitButton;
        }

        public override async UniTask<bool> Run()
        {
            for (int i = 0; i < _data.TurnCount; i++)
            {
                UniTask boardInputTask = _loop.ProcessUserInput();
                UniTask backButtonClickTask = _exitButton.WaitForClick();

                await UniTask.WhenAny(boardInputTask, backButtonClickTask);

                if (backButtonClickTask.IsCompleted())
                    return true;

                await _loop.ProcessBoard();
            }

            return false;
        }
    }
}