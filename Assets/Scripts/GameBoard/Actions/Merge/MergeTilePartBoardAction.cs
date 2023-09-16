using System;
using Cysharp.Threading.Tasks;
using Tiles;
using Tiles.Containers;
using Tiles.Data;
using UnityEngine;

namespace GameBoard.Actions.Merge
{
    public class MergeTilePartBoardAction : BoardAction
    {
        private MixedTileContainer _tileContainer;

        public MergeTilePartBoardAction(MixedTileContainer tileContainer, Board board) : base(board)
        {
            _tileContainer = tileContainer;
        }

        public override async UniTask Run()
        {
            MixedTile tile = _tileContainer.MixedTile;

            Board.FreeTile(tile);

            Board.Grid.AddTileOffGrid(tile);
            Board.Grid.MoveTileOnTop(tile);
            
            await PlayMergePartAnimation();

            CreateLeftoverTile();

            tile.Dispose();
        }

        private UniTask PlayMergePartAnimation()
        {
            MixedTile tile = _tileContainer.MixedTile;
            Vector2 targetPosition = GetTargetPosition();

            return tile.PlayMergePartAnimation(_tileContainer.PartType, targetPosition);
        }

        private Vector2 GetTargetPosition()
        {
            Vector2Int targetTileBoardPosition = _tileContainer.Target.Tile.BoardPosition;
            Vector2 targetTileWorldPosition = Board.GetWorldPosition(targetTileBoardPosition);

            return targetTileWorldPosition;
        }

        private void CreateLeftoverTile()
        {
            RegularTileData leftoverTile = GetLeftoverTile();
            Vector2Int tilePosition = _tileContainer.Tile.BoardPosition;

            Board.CreateTile(leftoverTile, tilePosition);
        }

        private RegularTileData GetLeftoverTile()
        {
            switch (_tileContainer.PartType)
            {
                case MixedTilePartType.Top:
                    return GetLeftoverTile(_tileContainer.MixedTile.Bottom);
                case MixedTilePartType.Bottom:
                    return GetLeftoverTile(_tileContainer.MixedTile.Top);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private RegularTileData GetLeftoverTile(MixedTileModel model)
        {
            RegularTileData leftoverTile = new RegularTileData(model.Value, model.Color, _tileContainer.MixedTile.Age);
            return leftoverTile;
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}