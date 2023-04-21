using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tiles;
using Tiles.Containers;
using UnityEngine;

namespace GameBoard.Turns.Merge
{
    public class MergeRegularTileBoardTurn : MergeBoardTurn
    {
        private RegularTile _tile;
        private IValueTileContainer[] _mergeTileContainers;
        private Board _board;

        public MergeRegularTileBoardTurn(RegularTile tile, IValueTileContainer[] mergeTileContainers, Board board)
        {
            _tile = tile;
            _mergeTileContainers = mergeTileContainers;
            _board = board;
        }

        public override async UniTask Run()
        {
            Debug.Log($"{GetType()} turn START");

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            IEnumerable<UniTask> mergeTasks = RunMergeTasks(_mergeTileContainers, _board);
            await UniTask.WhenAll(mergeTasks);

            _tile.IncrementValue();

            Debug.Log($"{GetType()} turn FINISH");
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}