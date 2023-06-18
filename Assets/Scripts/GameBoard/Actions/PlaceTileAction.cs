using System;
using Cysharp.Threading.Tasks;
using Tiles;
using UnityEngine;

namespace GameBoard.Actions
{
    public class PlaceTileAction : BoardAction
    {
        private Board _board;
        private Tile _tile;
        private Vector2Int _boardPosition;

        public PlaceTileAction(Board board, Tile tile, Vector2Int boardPosition)
        {
            _board = board;
            _tile = tile;
            _boardPosition = boardPosition;
        }

        public override async UniTask Run()
        {
            _board.Grid.AddTileOffGrid(_tile);

            Vector2 worldPosition = _board.GetWorldPosition(_boardPosition);
            await _tile.Appear(worldPosition);

            _board.PlaceTile(_tile, _boardPosition);
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}