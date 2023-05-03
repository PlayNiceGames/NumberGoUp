using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameBoard.Actions.Merge;
using GameScore;
using Tiles;
using Tiles.Containers;

namespace GameBoard.Turns.Merge
{
    public abstract class MergeBoardTurn : BoardTurn
    {
        protected ScoreSystem _scoreSystem;

        protected MergeBoardTurn(Board board, ScoreSystem scoreSystem) : base(board)
        {
            _scoreSystem = scoreSystem;
        }

        protected IEnumerable<UniTask> RunMergeTasks(IEnumerable<IValueTileContainer> mergeTiles, Board board)
        {
            foreach (IValueTileContainer mergedTile in mergeTiles)
            {
                if (mergedTile.Tile is RegularTile mergeRegularTile)
                {
                    MergeRegularTileBoardAction action = new MergeRegularTileBoardAction(mergeRegularTile, board);

                    yield return action.Run();
                }
                else if (mergedTile is MixedTileContainer mergeMixedTileContainer)
                {
                    MergeMixedTileBoardAction action = new MergeMixedTileBoardAction(mergeMixedTileContainer, board);

                    yield return action.Run();
                }
            }
        }

        protected void IncrementContainerValue(IValueTileContainer container, int valueDelta = 1)
        {
            int startingValue = container.GetValue();

            container.IncrementValue(valueDelta);

            _scoreSystem.IncrementScoreForMerge(startingValue, valueDelta, container);
        }
    }
}