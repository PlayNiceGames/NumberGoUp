using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tiles;

namespace GameBoard.Actions
{
    public class ResizeBoardAction : BoardAction
    {
        private int _newSize;

        private TileFactory _factory;

        public ResizeBoardAction(int newSize, TileFactory factory, Board board) : base(board)
        {
            _newSize = newSize;
            _factory = factory;
        }

        public override async UniTask Run()
        {
            if (Board.Size == _newSize)
                return;

            Board.UpdateBoardSize(_newSize);

            await Board.Grid.WaitForGridUpdate();

            await PlaceEmptyTiles();
        }

        private async UniTask PlaceEmptyTiles()
        {
            List<UniTask> placeTileTasks = new List<UniTask>();

            foreach (VoidTile voidTile in Board.GetAllTiles<VoidTile>())
            {
                EmptyTile emptyTile = _factory.InstantiateTile<EmptyTile>();
                PlaceTileAction placeTileAction = new PlaceTileAction(emptyTile, voidTile.BoardPosition, Board);

                UniTask placeTileTask = placeTileAction.Run();
                placeTileTasks.Add(placeTileTask);

                await emptyTile.AppearAnimation.WaitForTilesAppearDelay();
            }

            await UniTask.WhenAll(placeTileTasks);
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}