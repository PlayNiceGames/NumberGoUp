using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Tiles;
using UnityEngine;

namespace GameBoard.Actions
{
    public class MergeTileBoardAction : BoardAction
    {
        private Board _board;
        private ValueTile _tile;
        private ValueTile[] _mergeTiles;

        public MergeTileBoardAction(Board board, ValueTile tile, ValueTile[] mergeTiles)
        {
            _board = board;
            _tile = tile;
            _mergeTiles = mergeTiles;
        }

        public override async UniTask Run()
        {
            Debug.Log("RUN ACTION START");

            await UniTask.Delay(TimeSpan.FromSeconds(2));

            foreach (ValueTile mergedTile in _mergeTiles)
            {
                if (mergedTile is RegularTile mergeRegularTile)
                    MergeRegularTile(mergeRegularTile).Forget();
                if (mergedTile is MixedTile mergeMixedTile)
                    MergeMixedTile(mergeMixedTile).Forget();
            }

            if (_tile is RegularTile regularTile)
                regularTile.IncrementValue();

            Debug.Log("RUN ACTION FINISH");
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }

        private UniTask MergeRegularTile(RegularTile tile)
        {
            _board.PlaceEmptyTile(tile.BoardPosition); //TODO free and animate tile

            return UniTask.CompletedTask;
        }

        private UniTask MergeMixedTile(MixedTile tile)
        {
            _board.PlaceEmptyTile(tile.BoardPosition);

            return UniTask.CompletedTask;
        }
    }
}