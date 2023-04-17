using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tiles;
using UnityEngine;

namespace GameBoard.Turns.Merge
{
    public class MergeRegularTileBoardTurn : MergeBoardTurn
    {
        private RegularTile _tile;
        private ValueTile[] _mergeTiles;
        private Board _board;

        public MergeRegularTileBoardTurn(RegularTile tile, ValueTile[] mergeTiles, Board board)
        {
            _tile = tile;
            _mergeTiles = mergeTiles;
            _board = board;
        }

        public override async UniTask Run()
        {
            Debug.Log($"{GetType()} turn START");

            await UniTask.Delay(TimeSpan.FromSeconds(2));

            IEnumerable<UniTask> mergeTasks = RunMergeTasks(_mergeTiles, _board);
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