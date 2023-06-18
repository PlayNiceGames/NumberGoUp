using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameBoard.Actions;
using GameScore;
using Tiles.Containers;

namespace GameBoard.Turns.Merge
{
    public class DoubleMergeBoardTurn : MergeBoardTurn
    {
        private MergeContainer _firstContainer;
        private MergeContainer _secondContainer;
        private IEnumerable<MergeContainer> _mergeTileContainers;

        public DoubleMergeBoardTurn(MergeContainer firstContainer, MergeContainer secondContainer, IEnumerable<MergeContainer> mergeTileContainers, Board board, ScoreSystem scoreSystem) : base(board, scoreSystem)
        {
            _firstContainer = firstContainer;
            _secondContainer = secondContainer;
            _mergeTileContainers = mergeTileContainers;
            _board = board;
            ScoreSystem = scoreSystem;
        }

        public override async UniTask Run()
        {
            IEnumerable<BoardAction> mergeActions = GetMergeActions(_mergeTileContainers, _board);
            await RunMergeActions(mergeActions);

            IncrementContainerValue(_firstContainer);
            IncrementContainerValue(_secondContainer);
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}