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
        private IValueTileContainer _firstContainer;
        private IValueTileContainer _secondContainer;
        private IEnumerable<IValueTileContainer> _mergeTileContainers;
        private Board _board;
        private ScoreSystem _scoreSystem;

        public DoubleMergeBoardTurn(IValueTileContainer firstContainer, IValueTileContainer secondContainer, IEnumerable<IValueTileContainer> mergeTileContainers, Board board, ScoreSystem scoreSystem)
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

            _firstContainer.IncrementValue();
            _scoreSystem.IncrementScoreForMerge(_firstContainer.GetValue());

            _secondContainer.IncrementValue();
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}