using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tiles;

namespace GameBoard.Turns
{
    public class EraserBoardTurn : BoardTurn
    {
        private readonly List<EraserTile> _eraserTiles;

        public EraserBoardTurn(Board board, List<EraserTile> eraserTiles) : base(board)
        {
            _eraserTiles = eraserTiles;
        }

        public override UniTask Run()
        {
            foreach (EraserTile eraserTile in _eraserTiles)
            {
                _board.ClearTile(eraserTile.BoardPosition);
            }

            return UniTask.CompletedTask;
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}