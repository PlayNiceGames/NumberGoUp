using System.Collections.Generic;
using System.Linq;
using GameBoard.Turns;
using GameBoard.Turns.Merge;
using GameScore;
using Tiles;
using Tiles.Containers;
using Utils;

namespace GameBoard.Rules.Merge
{
    public class SimpleMergeBoardRule : MergeBoardRule
    {
        public SimpleMergeBoardRule(Board board, ScoreSystem scoreSystem) : base(board, scoreSystem)
        {
        }

        public override BoardTurn GetTurn()
        {
            IEnumerable<MergeContainer> sortedTiles = GetSortedTileContainers();

            foreach (MergeContainer container in sortedTiles)
            {
                List<MergeContainer> mergeableContainers = GetMergeableContainers(container);

                if (mergeableContainers == null)
                    continue;

                SimpleMergeBoardTurn bothPartsTurn = TryGetBothPartsTurn(container, mergeableContainers);

                return bothPartsTurn ?? new SimpleMergeBoardTurn(container, mergeableContainers, _board, _scoreSystem);
            }

            return null;
        }

        private SimpleMergeBoardTurn TryGetBothPartsTurn(MergeContainer target, IEnumerable<MergeContainer> mergeableContainers)
        {
            if (target is not MixedTileContainer mixedContainer)
                return null;

            MixedTileContainer otherPartContainer = mixedContainer.GetOtherPart();

            List<MergeContainer> otherPartMergeableContainers = GetMergeableContainers(otherPartContainer);

            if (otherPartMergeableContainers == null)
                return null;

            MixedTile tile = mixedContainer.MixedTile;
            MixedTileContainer bothPartsContainer = new MixedTileContainer(tile, null, MixedTilePartType.Both);

            IEnumerable<MergeContainer> bothPartsMergeableContainers = mergeableContainers.Select(mergeableContainer =>
                    MergeContainer.TryCreateMergeContainer(mergeableContainer.Tile, bothPartsContainer))
                .Where(container => container != null);

            return new SimpleMergeBoardTurn(bothPartsContainer, bothPartsMergeableContainers, _board, _scoreSystem);
        }

        private List<MergeContainer> GetMergeableContainers(MergeContainer container)
        {
            List<ValueTile> validNearbyTiles = _board.GetNearbyTiles(container.Tile.BoardPosition).OfType<ValueTile>().ToList();
            List<MergeContainer> tiles = GetNearbyMergeableTiles(container, validNearbyTiles);

            return tiles;
        }

        private List<MergeContainer> GetNearbyMergeableTiles(MergeContainer target, List<ValueTile> validNearbyTiles)
        {
            IEnumerable<MergeContainer> validNearbyTileContainers = validNearbyTiles.Select(tile => MergeContainer.TryCreateMergeContainer(tile, target))
                .Where(container => container != null);
            IEnumerable<IEnumerable<MergeContainer>> combinations = validNearbyTileContainers.Combinations();

            foreach (IEnumerable<MergeContainer> combination in combinations)
            {
                List<MergeContainer> combinationList = combination.ToList();

                int sum = combinationList.Sum(combinationContainer => combinationContainer.GetValue());
                if (sum == target.GetValue())
                    return combinationList;
            }

            return null;
        }
    }
}