using System.Collections.Generic;
using System.Linq;
using Analytics;
using GameBoard.Turns;
using GameBoard.Turns.Merge;
using GameScore;
using Tiles;
using Tiles.Containers;

namespace GameBoard.Rules.Merge
{
    public class CenterMergeBoardRule : MergeBoardRule
    {
        private readonly AnalyticsService _analytics;

        public CenterMergeBoardRule(Board board, ScoreSystem scoreSystem, AnalyticsService analytics) : base(board, scoreSystem)
        {
            _scoreSystem = scoreSystem;
            _analytics = analytics;
        }

        public override BoardTurn GetTurn()
        {
            IEnumerable<MergeContainer> allMergeableContainers = GetSortedTileContainers();

            foreach (MergeContainer target in allMergeableContainers)
            {
                List<ValueTile> nearbyTiles = _board.GetNearbyTiles(target.Tile.BoardPosition).OfType<ValueTile>().ToList();
                List<MergeContainer> mergeableTileContainers = GetMergeableTiles(target, nearbyTiles).ToList();

                int sameTilesCount = mergeableTileContainers.Count;
                if (sameTilesCount > 1)
                    return new CenterMergeBoardTurn(target, mergeableTileContainers, _board, _scoreSystem, _analytics);
            }

            return null;
        }

        private IEnumerable<MergeContainer> GetMergeableTiles(MergeContainer target, List<ValueTile> nearbyTiles)
        {
            foreach (ValueTile nearbyTile in nearbyTiles)
            {
                MergeContainer mergeContainer = MergeContainer.TryCreateMergeContainer(nearbyTile, target);

                if (mergeContainer == null)
                    continue;

                if (mergeContainer.GetValue() == target.GetValue())
                    yield return mergeContainer;
            }
        }

        private MergeContainer GetContainer(ValueTile tile)
        {
            if (tile is RegularTile regularTile)
                return new RegularTileContainer(regularTile, null);
            if (tile is MixedTile mixedTileContainer)
                return new MixedTileContainer(mixedTileContainer, null, MixedTilePartType.Both);

            return null;
        }
    }
}