using System;
using System.Collections.Generic;
using System.Linq;
using Analytics;
using Cysharp.Threading.Tasks;
using GameAnalytics.Events.Game;
using GameBoard.Actions;
using GameScore;
using Tiles.Containers;

namespace GameBoard.Turns.Merge
{
    public class CenterMergeBoardTurn : MergeBoardTurn
    {
        public override MergeType Type => MergeType.Center;

        private MergeContainer _tileContainer;
        private IEnumerable<MergeContainer> _mergeTileContainers;
        private AnalyticsService _analytics;

        public CenterMergeBoardTurn(MergeContainer tileContainer, IEnumerable<MergeContainer> mergeTileContainers, Board board, ScoreSystem scoreSystem, AnalyticsService analytics) : base(board, scoreSystem)
        {
            _tileContainer = tileContainer;
            _mergeTileContainers = mergeTileContainers;
            _board = board;
            _analytics = analytics;
        }

        public override async UniTask Run()
        {
            int containersCount = _mergeTileContainers.Count();
            int newValue = _tileContainer.GetValue() + containersCount;

            PlayMergeSound(newValue);

            int scoreDelta = ScoreSystem.GetScoreForMerge(_tileContainer, containersCount);
            UniTask scoreTask = ScoreSystem.IncrementScore(scoreDelta);

            IEnumerable<BoardAction> mergeActions = GetMergeActions(_mergeTileContainers, _board);
            UniTask mergeTask = RunMergeActions(mergeActions);

            await UniTask.WhenAll(mergeTask, scoreTask);

            _tileContainer.IncrementValue(containersCount);

            _analytics.Send(new SpecialMergeEvent(Type, ScoreSystem.Score, _tileContainer.GetValue()));
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}