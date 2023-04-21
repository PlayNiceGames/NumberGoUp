using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tiles;
using Tiles.Containers;
using UnityEngine;

namespace GameBoard.Turns.Merge
{
    public class MergeMixedTileBoardTurn : MergeBoardTurn
    {
        private IValueTileContainer _tileContainer;
        private IValueTileContainer[] _mergeTileContainers;
        private Board _board;

        public MergeMixedTileBoardTurn(IValueTileContainer tileContainer, IValueTileContainer[] mergeTileContainers, Board board)
        {
            _tileContainer = tileContainer;
            _mergeTileContainers = mergeTileContainers;
            _board = board;
        }

        public override async UniTask Run()
        {
            Debug.Log($"{GetType()} turn START");

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            IEnumerable<UniTask> mergeTasks = RunMergeTasks(_mergeTileContainers, _board);
            await UniTask.WhenAll(mergeTasks);

            _tileContainer.IncrementValue();

            Debug.Log($"{GetType()} turn FINISH");
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}