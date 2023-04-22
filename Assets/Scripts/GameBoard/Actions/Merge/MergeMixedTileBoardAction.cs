using System;
using Cysharp.Threading.Tasks;
using Tiles;
using Tiles.Containers;
using Tiles.Data;

namespace GameBoard.Actions.Merge
{
    public class MergeMixedTileBoardAction : BoardAction
    {
        private MixedTileContainer _tileContainer;
        private Board _board;

        public MergeMixedTileBoardAction(MixedTileContainer tileContainer, Board board)
        {
            _tileContainer = tileContainer;
            _board = board;
        }

        public override UniTask Run()
        {
            RegularTileData leftoverTile = GetLeftoverTile();

            if (leftoverTile == null)
                _board.CreateEmptyTile(_tileContainer.Tile.BoardPosition);
            else
                _board.CreateTile(leftoverTile, _tileContainer.Tile.BoardPosition); //TODO free tile from board and animate it

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
                case MixedTilePartType.Both:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private RegularTileData GetLeftoverTile(MixedTileModel model)
        {
            RegularTileData leftoverTile = new RegularTileData(model.Value, model.Color);
            leftoverTile.Age = _tileContainer.MixedTile.Age;

            return leftoverTile;
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}