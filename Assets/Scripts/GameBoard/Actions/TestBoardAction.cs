using System;
using Cysharp.Threading.Tasks;
using Tiles;

namespace GameBoard.Actions
{
    public class TestBoardAction : BoardAction
    {
        private RegularTile _tile;

        public TestBoardAction(RegularTile tile)
        {
            _tile = tile;
        }

        public override async UniTask Run()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2));

            _tile.IncrementValue(1);
        }

        public override UniTask Undo()
        {
            _tile.IncrementValue(-1);

            return UniTask.CompletedTask;
        }
    }
}