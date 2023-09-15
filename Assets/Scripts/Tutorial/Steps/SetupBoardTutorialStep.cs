using System;
using Cysharp.Threading.Tasks;
using GameBoard;
using GameBoard.Actions;
using Tiles;
using Tutorial.Steps.Data;

namespace Tutorial.Steps
{
    [Serializable]
    public class SetupBoardTutorialStep : TutorialStep
    {
        private SetupBoardTutorialStepData _data;

        private TileFactory _factory;
        private Board _board;

        public SetupBoardTutorialStep(SetupBoardTutorialStepData data, TileFactory factory, Board board)
        {
            _data = data;
            _factory = factory;
            _board = board;
        }

        public override async UniTask<TutorialStepResult> Run()
        {
            BoardData boardData = _data.BoardConfiguration;

            SetupBoardAction setupBoardAction = new SetupBoardAction(boardData, _factory, _board);

            await setupBoardAction.Run();

            return TutorialStepResult.Completed;
        }
    }
}