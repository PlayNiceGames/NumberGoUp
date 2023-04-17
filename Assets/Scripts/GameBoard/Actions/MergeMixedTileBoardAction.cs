using System;
using Cysharp.Threading.Tasks;
using Tiles;

namespace GameBoard.Actions
{
    public class MergeMixedTileBoardAction : BoardAction
    {
        private MixedTile _tile;
        private Board _board;
        private ValueTile[] _mergeTiles;

        public MergeMixedTileBoardAction(MixedTile tile, Board board)
        {
            _board = board;
            _tile = tile;
        }

        public override UniTask Run()
        {
            _board.PlaceEmptyTile(_tile.BoardPosition); //TODO free tile from board and animate it
            
            return UniTask.CompletedTask;
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}