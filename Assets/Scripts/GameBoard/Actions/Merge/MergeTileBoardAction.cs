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

        public MergeTileBoardAction(MergeContainer tileContainer, Board board) : base(board)
        {
            _tileContainer = tileContainer;
        }

        public override async UniTask Run()
        {
            ValueTile tile = _tileContainer.Tile;
            MergeContainer target = _tileContainer.Target;

            Board.FreeTile(tile);

            Board.Grid.AddTileOffGrid(tile);
            Board.Grid.MoveTileOnTop(tile);

            Vector2 worldPosition = Board.GetWorldPosition(target.Tile.BoardPosition);
            await tile.Merge(worldPosition);

            tile.Dispose();
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}