using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameScore;
using Tiles.Containers;
using UnityEngine;

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
            _scoreSystem = scoreSystem;
        }

        public override async UniTask Run()
        {
            Debug.Log($"{GetType()} turn");

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            IEnumerable<UniTask> mergeTasks = RunMergeTasks(_mergeTileContainers, _board);
            await UniTask.WhenAll(mergeTasks);

            IncrementContainerValue(_tileContainer);
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}