using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameBoard.Actions;
using GameScore;
using Tiles.Containers;

namespace GameBoard.Turns.Merge
{
    public class SimpleMergeBoardTurn : MergeBoardTurn
    {
        private MergeContainer _tileContainer;
        private IEnumerable<MergeContainer> _mergeTileContainers;

        public SimpleMergeBoardTurn(MergeContainer tileContainer, IEnumerable<MergeContainer> mergeTileContainers, Board board, ScoreSystem scoreSystem) : base(board, scoreSystem)
        {
            _tileContainer = tileContainer;
            _mergeTileContainers = mergeTileContainers;
            _board = board;
        }

        public override async UniTask Run()
        {
            IEnumerable<BoardAction> mergeActions = GetMergeActions(_mergeTileContainers, _board);
            await RunMergeActions(mergeActions);

            IncrementContainerValue(_tileContainer);
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}