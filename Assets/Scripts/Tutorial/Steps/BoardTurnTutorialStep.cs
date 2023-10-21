using System.Threading;
using Cysharp.Threading.Tasks;
using GameActions;
using GameLoop;
using Tiles;
using Tutorial.Steps.Data;
using Utils;

namespace Tutorial.Steps
{
    public class BoardTurnTutorialStep : TutorialStep
    {
        private readonly BoardTurnTutorialStepData _data;

        private readonly BoardInput _boardInput;
        private readonly BoardGameLoop _boardLoop;
        private readonly GameButton _exitButton;

        public BoardTurnTutorialStep(BoardTurnTutorialStepData data, BoardInput boardInput, BoardGameLoop boardLoop, GameButton exitButton)
        {
            _data = data;
            _boardInput = boardInput;
            _boardLoop = boardLoop;
            _exitButton = exitButton;
        }

        public override async UniTask<TutorialStepResult> Run()
        {
            for (int i = 0; i < _data.TurnCount; i++)
            {
                bool shouldExit = await ProcessInput();

                if (shouldExit)
                    return TutorialStepResult.ExitToMenu;

                await _boardLoop.ProcessBoard();
            }

            return TutorialStepResult.Completed;
        }

        private async UniTask<bool> ProcessInput()
        {
            while (true)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = cancellationTokenSource.Token;
                
                UniTask<Tile> boardInputTask = _boardInput.WaitUntilTileClicked(cancellationToken);
                UniTask backButtonClickTask = _exitButton.WaitForClick(cancellationToken);

                await UniTask.WhenAny(boardInputTask, backButtonClickTask);
                
                cancellationTokenSource.Cancel();

                if (boardInputTask.AsUniTask().IsCompleted())
                {
                    Tile tile = await boardInputTask;
                    bool isCorrectTile = await _boardLoop.ProcessInput(tile);

                    if (!isCorrectTile)
                        continue;

                    return false;
                }

                if (backButtonClickTask.IsCompleted())
                    return true;
            }
        }
    }
}