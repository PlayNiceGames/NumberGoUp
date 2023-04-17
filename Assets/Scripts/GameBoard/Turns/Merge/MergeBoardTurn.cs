using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameBoard.Actions;
using Tiles;

namespace GameBoard.Turns.Merge
{
    public abstract class MergeBoardTurn : BoardTurn
    {
        protected IEnumerable<UniTask> RunMergeTasks(ValueTile[] mergeTiles, Board board)
        {
            foreach (ValueTile mergedTile in mergeTiles)
            {
                if (mergedTile is RegularTile mergeRegularTile)
                {
                    MergeRegularTileBoardAction action = new MergeRegularTileBoardAction(mergeRegularTile, board);

                    yield return action.Run();
                }
                else if (mergedTile is MixedTile mergeMixedTile)
                {
                    MergeMixedTileBoardAction action = new MergeMixedTileBoardAction(mergeMixedTile, board);

                    yield return action.Run();
                }
            }
        }
    }
}