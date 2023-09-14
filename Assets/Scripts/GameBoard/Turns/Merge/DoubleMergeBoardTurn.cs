using System;
using System.Collections.Generic;
using System.Linq;
using Analytics;
using Cysharp.Threading.Tasks;
using GameAnalytics.Events.Game;
using GameBoard.Actions;
using GameScore;
using ServiceLocator;
using Tiles;
using Tiles.Containers;
using Tiles.Data;

namespace GameBoard.Turns.Merge
{
    public class DoubleMergeBoardTurn : MergeBoardTurn
    {
        public override MergeType Type => MergeType.Double;

        private MergeContainer _firstContainer;
        private MergeContainer _secondContainer;
        private List<MergeContainer> _mergeTileContainers;
        private TileFactory _factory;

        private AnalyticsService _analytics;

        public DoubleMergeBoardTurn(MergeContainer firstContainer, MergeContainer secondContainer, IEnumerable<MergeContainer> mergeTileContainers, Board board, TileFactory factory, ScoreSystem scoreSystem, AnalyticsService analytics) : base(board, scoreSystem)
        {
            _firstContainer = firstContainer;
            _secondContainer = secondContainer;
            _mergeTileContainers = mergeTileContainers.ToList();
            _board = board;
            _factory = factory;

            _analytics = GlobalServices.Get<AnalyticsService>();
        }

        public override async UniTask Run()
        {
            int newValue = _firstContainer.GetValue() + 1;
            PlayMergeSound(newValue);

            int scoreDelta = ScoreSystem.GetScoreForMerge(_firstContainer) + ScoreSystem.GetScoreForMerge(_secondContainer);
            UniTask scoreTask = ScoreSystem.IncrementScore(scoreDelta);

            IEnumerable<MergeContainer> fakeContainers = SpawnFakeContainers();
            List<MergeContainer> allContainers = _mergeTileContainers.Concat(fakeContainers).ToList();

            IEnumerable<BoardAction> mergeActions = GetMergeActions(allContainers, _board);
            UniTask mergeTask = RunMergeActions(mergeActions);

            await UniTask.WhenAll(mergeTask, scoreTask);

            _firstContainer.IncrementValue();
            _secondContainer.IncrementValue();

            _analytics.Send(new SpecialMergeEvent(Type, ScoreSystem.Score, _firstContainer.GetValue()));
        }

        private IEnumerable<MergeContainer> SpawnFakeContainers()
        {
            foreach (MergeContainer mergeContainer in _mergeTileContainers)
            {
                TileData fakeTileData = mergeContainer.Tile.GetData();

                ValueTile fakeTile = (ValueTile) _factory.InstantiateTile(fakeTileData);
                fakeTile.transform.position = mergeContainer.Tile.transform.position;

                MergeContainer fakeTileContainer = MergeContainer.TryCreateMergeContainer(fakeTile, _secondContainer);

                yield return fakeTileContainer;
            }
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}