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
        private Board _board;

        public MergeTilePartBoardAction(MixedTileContainer tileContainer, Board board)
        {
            _tileContainer = tileContainer;
            _board = board;
        }

        public override UniTask Run()
        {
            RegularTileData leftoverTile = GetLeftoverTile();
            Vector2Int tilePosition = _tileContainer.Tile.BoardPosition;

            _board.CreateTile(leftoverTile, tilePosition); //TODO free tile from board and animate it

            return UniTask.CompletedTask;
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