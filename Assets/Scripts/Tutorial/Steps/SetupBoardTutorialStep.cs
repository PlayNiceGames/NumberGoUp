using System;
using Cysharp.Threading.Tasks;
using GameBoard;
using Tutorial.Data;

namespace Tutorial
{
    [Serializable]
    public class SetupBoardTutorialStep : TutorialStep
    {
        private SetupBoardTutorialStepData _data;

        private Board _board;

        public SetupBoardTutorialStep(SetupBoardTutorialStepData data, Board board)
        {
            _data = data;
            _board = board;
        }

        public override UniTask Run()
        {
            BoardData boardData = _data.BoardConfiguration;
            _board.SetData(boardData);

            return UniTask.CompletedTask;
        }
    }
}