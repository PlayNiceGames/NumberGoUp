using System;
using Cysharp.Threading.Tasks;
using Tiles;
using Tiles.Containers;
using UnityEngine;

namespace GameBoard.Actions.Merge
{
    public class MergeTileBoardAction : BoardAction
    {
        private MergeContainer _tileContainer;
        private Board _board;

        public MergeTileBoardAction(MergeContainer tileContainer, Board board)
        {
            _tileContainer = tileContainer;
            _board = board;
        }

        public override async UniTask Run()
        {
            ValueTile tile = _tileContainer.Tile;
            ValueTile target = _tileContainer.Target;

            _board.FreeTile(tile.BoardPosition);
            _board.Grid.AddTileOffGrid(tile);
            _board.Grid.MoveTileOnTop(tile);

            Vector2 worldPosition = _board.GetWorldPosition(target.BoardPosition);
            await tile.Merge(worldPosition);

            tile.Dispose();
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}