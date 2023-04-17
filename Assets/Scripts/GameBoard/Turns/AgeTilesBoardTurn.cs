using Cysharp.Threading.Tasks;
using Tiles;
using UnityEngine;

namespace GameBoard.Turns
{
    public class AgeTilesBoardTurn : BoardTurn
    {
        private Board _board;

        public AgeTilesBoardTurn(Board board)
        {
            _board = board;
        }

        public override UniTask Run()
        {
            Debug.Log($"{GetType()} turn START");
            
            foreach (ValueTile tile in _board.GetAllTiles<ValueTile>())
            {
                tile.Age++;
            }

            return UniTask.CompletedTask;
        }

        public override UniTask Undo()
        {
            foreach (ValueTile tile in _board.GetAllTiles<ValueTile>())
            {
                tile.Age--;
            }

            return UniTask.CompletedTask;
        }
    }
}