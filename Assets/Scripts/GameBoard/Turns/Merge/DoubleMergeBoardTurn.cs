using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameAudio;
using GameBoard.Actions;
using GameScore;
using Tiles;
using Tiles.Containers;
using Tiles.Data;

namespace GameBoard.Turns.Merge
{
    public class DoubleMergeBoardTurn : MergeBoardTurn
    {
        private MergeContainer _firstContainer;
        private MergeContainer _secondContainer;
        private List<MergeContainer> _mergeTileContainers;
        private TileFactory _factory;

        public DoubleMergeBoardTurn(MergeContainer firstContainer, MergeContainer secondContainer, IEnumerable<MergeContainer> mergeTileContainers, Board board, TileFactory factory, ScoreSystem scoreSystem) : base(board, scoreSystem)
        {
            _firstContainer = firstContainer;
            _secondContainer = secondContainer;
            _mergeTileContainers = mergeTileContainers.ToList();
            _board = board;
            _factory = factory;
        }

        public override async UniTask Run()
        {
            int newValue = _firstContainer.GetValue() + 1;
            GameSounds.PlayMerge(newValue);

            int scoreDelta = ScoreSystem.GetScoreForMerge(_firstContainer) + ScoreSystem.GetScoreForMerge(_secondContainer);
            UniTask scoreTask = ScoreSystem.IncrementScore(scoreDelta);

            IEnumerable<MergeContainer> fakeContainers = SpawnFakeContainers();
            List<MergeContainer> allContainers = _mergeTileContainers.Concat(fakeContainers).ToList();

            IEnumerable<BoardAction> mergeActions = GetMergeActions(allContainers, _board);
            UniTask mergeTask = RunMergeActions(mergeActions);

            await UniTask.WhenAll(mergeTask, scoreTask);

            _firstContainer.IncrementValue();
            _secondContainer.IncrementValue();
        }

        private IEnumerable<MergeContainer> SpawnFakeContainers()
        {
            foreach (MergeContainer mergeContainer in _mergeTileContainers)
            {
                TileData fakeTileData = mergeContainer.Tile.GetData();

                ValueTile fakeTile = (ValueTile) _factory.InstantiateTile(fakeTileData);
                fakeTile.transform.position = mergeContainer.Tile.transform.position;

                MergeContainer fakeTileContainer = MergeContainer.GetMergeContainer(fakeTile, _secondContainer);

                yield return fakeTileContainer;
            }
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}