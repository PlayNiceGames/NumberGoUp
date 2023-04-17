using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tiles;
using UnityEngine;

namespace GameBoard.Turns.Merge
{
    public class MergeMixedTileBoardTurn : MergeBoardTurn
    {
        private MixedTile _tile;
        private MixedTileModel _tilePart;
        private ValueTile[] _mergeTiles;
        private Board _board;

        public MergeMixedTileBoardTurn(MixedTile tile, MixedTileModel tilePart, ValueTile[] mergeTiles, Board board)
        {
            _tile = tile;
            _tilePart = tilePart;
            _mergeTiles = mergeTiles;
            _board = board;
        }

        public override async UniTask Run()
        {
            Debug.Log($"{GetType()} turn START");

            await UniTask.Delay(TimeSpan.FromSeconds(2));

            IEnumerable<UniTask> mergeTasks = RunMergeTasks(_mergeTiles, _board);
            await UniTask.WhenAll(mergeTasks);

            _tilePart.IncrementValue();

            Debug.Log($"{GetType()} turn FINISH");
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}