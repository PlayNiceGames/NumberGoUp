using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
            IEnumerable<MergeContainer> fakeContainers = SpawnFakeContainers();
            List<MergeContainer> allContainers = _mergeTileContainers.Concat(fakeContainers).ToList();

            IEnumerable<BoardAction> mergeActions = GetMergeActions(allContainers, _board);
            await RunMergeActions(mergeActions);

            UniTask incrementFirstScoreTask = IncrementContainerValue(_firstContainer);
            UniTask incrementSecondScoreTask = IncrementContainerValue(_secondContainer);

            await UniTask.WhenAll(incrementFirstScoreTask, incrementSecondScoreTask);
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