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
            int scoreDelta = ScoreSystem.GetScoreForMerge(_tileContainer);
            UniTask scoreTask = ScoreSystem.IncrementScore(scoreDelta);

            IEnumerable<BoardAction> mergeActions = GetMergeActions(_mergeTileContainers, _board);
            UniTask mergeTask = RunMergeActions(mergeActions);

            await UniTask.WhenAll(mergeTask, scoreTask);

            _tileContainer.IncrementValue();
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}