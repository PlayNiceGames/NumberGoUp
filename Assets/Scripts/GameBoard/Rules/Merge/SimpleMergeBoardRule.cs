using System.Collections.Generic;
using System.Linq;
using GameBoard.Turns;
using GameBoard.Turns.Merge;
using Tiles;
using Tiles.Containers;
using Utils;

namespace GameBoard.Rules.Merge
{
    public class SimpleMergeBoardRule : MergeBoardRule
    {
        public SimpleMergeBoardRule(Board board) : base(board)
        {
        }

        public override BoardTurn GetTurn()
        {
            IEnumerable<IValueTileContainer> sortedTiles = GetSortedTileContainers();

            foreach (IValueTileContainer container in sortedTiles)
            {
                List<IValueTileContainer> mergeableContainers = GetMergeableContainers(container);

                if (mergeableContainers == null)
                    continue;

                SimpleMergeBoardTurn bothPartsTurn = TryGetBothPartsTurn(container, mergeableContainers);

                return bothPartsTurn ?? new SimpleMergeBoardTurn(container, mergeableContainers, _board);
            }

            return null;
        }

        private SimpleMergeBoardTurn TryGetBothPartsTurn(IValueTileContainer container, IEnumerable<IValueTileContainer> mergeableContainers)
        {
            if (container is not MixedTileContainer mixedContainer)
                return null;

            MixedTileContainer otherPartContainer = mixedContainer.GetOtherPart();

            List<IValueTileContainer> otherPartMergeableContainers = GetMergeableContainers(otherPartContainer);

            if (otherPartMergeableContainers == null)
                return null;

            MixedTileContainer bothPartsContainer = new MixedTileContainer(mixedContainer.MixedTile, MixedTilePartType.Both);

            IEnumerable<IValueTileContainer> bothPartsMergeableContainers = mergeableContainers.Select(mergeableContainer =>
                IValueTileContainer.GetMergeContainer(mergeableContainer.Tile, bothPartsContainer));

            return new SimpleMergeBoardTurn(bothPartsContainer, bothPartsMergeableContainers, _board);
        }

        private List<IValueTileContainer> GetMergeableContainers(IValueTileContainer container)
        {
            List<ValueTile> validNearbyTiles = _board.GetNearbyTiles(container.Tile.BoardPosition).OfType<ValueTile>().ToList();
            List<IValueTileContainer> tiles = GetNearbyMergeableTiles(container, validNearbyTiles);

            return tiles;
        }

        private List<IValueTileContainer> GetNearbyMergeableTiles(IValueTileContainer container, List<ValueTile> validNearbyTiles)
        {
            IEnumerable<IValueTileContainer> validNearbyTileContainers = validNearbyTiles.Select(tile => IValueTileContainer.GetMergeContainer(tile, container));
            IEnumerable<IValueTileContainer> mergeableTiles = validNearbyTileContainers.Where(tile => tile.IsMergeable(container));
            IEnumerable<IEnumerable<IValueTileContainer>> combinations = mergeableTiles.Combinations();

            foreach (IEnumerable<IValueTileContainer> combination in combinations)
            {
                List<IValueTileContainer> combinationList = combination.ToList();

                int sum = combinationList.Sum(combinationContainer => combinationContainer.GetValue());
                if (sum == container.GetValue())
                    return combinationList;
            }

            return null;
        }
    }
}