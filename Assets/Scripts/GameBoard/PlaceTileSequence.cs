using Cysharp.Threading.Tasks;
using Tiles;
using UnityEngine;

namespace GameBoard
{
    public class PlaceTileSequence
    {
        private Board _board;

        public PlaceTileSequence(Board board)
        {
            _board = board;
        }

        public async UniTask PlaceTile(Tile tile, Vector2Int boardPosition)
        {
            _board.Grid.AddTileOffGrid(tile);

            Vector2 worldPosition = _board.GetWorldPosition(boardPosition);
            await tile.Appear(worldPosition);

            _board.PlaceTile(tile, boardPosition);
        }
    }
}