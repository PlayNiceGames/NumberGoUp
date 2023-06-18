using System;
using Cysharp.Threading.Tasks;
using Tiles;
using UnityEngine;

namespace GameBoard.Actions
{
    public class PlaceTileAction : BoardAction
    {
        private Tile _tile;
        private Vector2Int _boardPosition;

        public PlaceTileAction(Tile tile, Vector2Int boardPosition, Board board) : base(board)
        {
            _tile = tile;
            _boardPosition = boardPosition;
        }

        public override async UniTask Run()
        {
            Board.Grid.AddTileOffGrid(_tile);
            Board.Grid.MoveTileOnTop(_tile);

            Vector2 worldPosition = Board.GetWorldPosition(_boardPosition);
            await _tile.Appear(worldPosition);

            Board.PlaceTile(_tile, _boardPosition);
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}