using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tiles;
using Tiles.Data;
using UnityEngine;

namespace GameBoard.Actions
{
    public class SetupBoardAction : BoardAction
    {
        private readonly BoardData _data;
        private readonly TileFactory _factory;

        public SetupBoardAction(BoardData data, TileFactory factory, Board board) : base(board)
        {
            _data = data;
            _factory = factory;
        }

        public override async UniTask Run()
        {
            List<UniTask> placeTileTasks = new List<UniTask>();

            int size = _data.Tiles.GetLength(0);

            Board.SetupBoard(size);

            await Board.Grid.WaitForGridUpdate();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    TileData tileData = _data.Tiles[i, j];

                    Tile tile = _factory.InstantiateTile(tileData);
                    Vector2Int boardPosition = new Vector2Int(i, j);
                    PlaceTileAction tileAppearAction = new PlaceTileAction(tile, boardPosition, Board);

                    UniTask placeTileTask = tileAppearAction.Run();
                    placeTileTasks.Add(placeTileTask);

                    if (tile.Type != TileType.Void)
                        await tile.AppearAnimation.WaitForTilesAppearDelay();
                }
            }

            await UniTask.WhenAll(placeTileTasks);
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}