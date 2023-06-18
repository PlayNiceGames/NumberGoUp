using System;
using Cysharp.Threading.Tasks;
using Tiles;

namespace GameBoard.Actions.Merge
{
    public class MergeTileBoardAction : BoardAction
    {
        private ValueTile _tile;
        private Board _board;

        public MergeTileBoardAction(ValueTile tile, Board board)
        {
            _board = board;
            _tile = tile;
        }

        public override UniTask Run()
        {
            _board.ClearTile(_tile.BoardPosition); //TODO free tile from board and animate it

            return UniTask.CompletedTask;
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}