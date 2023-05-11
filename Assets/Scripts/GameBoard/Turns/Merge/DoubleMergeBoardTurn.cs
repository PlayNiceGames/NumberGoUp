using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameScore;
using Tiles.Containers;
using UnityEngine;

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
            _scoreSystem = scoreSystem;
        }

        public override async UniTask Run()
        {
            Debug.Log($"{GetType()} turn");

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            IEnumerable<UniTask> mergeTasks = RunMergeTasks(_mergeTileContainers, _board);
            await UniTask.WhenAll(mergeTasks);

            IncrementContainerValue(_firstContainer);
            IncrementContainerValue(_secondContainer);
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}