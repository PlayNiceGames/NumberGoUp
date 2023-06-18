using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameBoard.Actions;
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

        protected IEnumerable<UniTask> RunMergeTasks(IEnumerable<MergeContainer> mergeTiles, Board board)
        {
            foreach (MergeContainer mergedTile in mergeTiles)
            {
                BoardAction action = null;

                if (mergedTile is RegularTileContainer mergeRegularTileContainer)
                {
                    action = new MergeTileBoardAction(mergeRegularTileContainer.Tile, board);
                }
                else if (mergedTile is MixedTileContainer mergeMixedTileContainer)
                {
                    if (mergeMixedTileContainer.PartType == MixedTilePartType.Both)
                        action = new MergeTileBoardAction(mergeMixedTileContainer.Tile, board);
                    else
                        action = new MergeTilePartBoardAction(mergeMixedTileContainer, board);
                }

                if (action != null)
                    yield return action.Run();
            }
        }

        protected void IncrementContainerValue(MergeContainer container, int valueDelta = 1)
        {
            int startingValue = container.GetValue();

            container.IncrementValue(valueDelta);

            _scoreSystem.IncrementScoreForMerge(startingValue, valueDelta, container);
        }
    }
}