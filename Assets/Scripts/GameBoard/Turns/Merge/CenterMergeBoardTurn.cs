using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameBoard.Actions;
using GameScore;
using Tiles.Containers;

namespace GameBoard.Turns.Merge
{
    public class CenterMergeBoardTurn : MergeBoardTurn
    {
        private MergeContainer _tileContainer;
        private IEnumerable<MergeContainer> _mergeTileContainers;

        public CenterMergeBoardTurn(MergeContainer tileContainer, IEnumerable<MergeContainer> mergeTileContainers, Board board, ScoreSystem scoreSystem) : base(board, scoreSystem)
        {
            _tileContainer = tileContainer;
            _mergeTileContainers = mergeTileContainers;
            _board = board;
        }

        public override async UniTask Run()
        {
            IEnumerable<BoardAction> mergeActions = GetMergeActions(_mergeTileContainers, _board);
            await RunMergeActions(mergeActions);

            int count = _mergeTileContainers.Count();
            await IncrementContainerValue(_tileContainer, count);
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}