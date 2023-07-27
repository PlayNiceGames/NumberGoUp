using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameAudio;
using GameBoard.Actions;
using GameBoard.Actions.Merge;
using GameScore;
using ServiceLocator;
using Tiles;
using Tiles.Containers;

namespace GameBoard.Turns.Merge
{
    public abstract class MergeBoardTurn : BoardTurn
    {
        protected ScoreSystem ScoreSystem;

        private Audio _audio;

        protected MergeBoardTurn(Board board, ScoreSystem scoreSystem) : base(board)
        {
            ScoreSystem = scoreSystem;

            _audio = GlobalServices.Get<Audio>();
        }

        protected IEnumerable<BoardAction> GetMergeActions(IEnumerable<MergeContainer> mergeTileContainers, Board board)
        {
            foreach (MergeContainer mergeTileContainer in mergeTileContainers)
            {
                BoardAction action = null;

                if (mergeTileContainer is RegularTileContainer mergeRegularTileContainer)
                {
                    action = new MergeTileBoardAction(mergeTileContainer, board);
                }
                else if (mergeTileContainer is MixedTileContainer mergeMixedTileContainer)
                {
                    if (mergeMixedTileContainer.PartType == MixedTilePartType.Both)
                        action = new MergeTileBoardAction(mergeTileContainer, board);
                    else
                        action = new MergeTilePartBoardAction(mergeMixedTileContainer, board);
                }

                if (action != null)
                    yield return action;
            }
        }

        protected UniTask RunMergeActions(IEnumerable<BoardAction> mergeActions)
        {
            IEnumerable<UniTask> actionTasks = mergeActions.Select(action => action.Run());
            return UniTask.WhenAll(actionTasks);
        }

        protected void PlayMergeSound(int newValue)
        {
            _audio.PlayMerge(newValue);
        }
    }
}