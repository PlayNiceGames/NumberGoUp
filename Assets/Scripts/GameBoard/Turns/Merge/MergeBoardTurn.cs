using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameBoard.Actions;
using GameBoard.Actions.Merge;
using Tiles;
using Tiles.Containers;

namespace GameBoard.Turns.Merge
{
    public abstract class MergeBoardTurn : BoardTurn
    {
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
    }
}