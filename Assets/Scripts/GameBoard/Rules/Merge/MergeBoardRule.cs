using System.Collections.Generic;
using System.Linq;
using GameScore;
using Tiles;
using Tiles.Containers;

namespace GameBoard.Rules.Merge
{
    public abstract class MergeBoardRule : BoardRule
    {
        protected ScoreSystem _scoreSystem;

        protected MergeBoardRule(Board board, ScoreSystem scoreSystem) : base(board)
        {
            _scoreSystem = scoreSystem;
        }

        protected IEnumerable<MergeContainer> GetAllMergeableTileParts(IEnumerable<ValueTile> tiles)
        {
            foreach (ValueTile tile in tiles)
            {
                if (tile is MixedTile mixedTile)
                {
                    yield return new MixedTileContainer(mixedTile, null, MixedTilePartType.Top);
                    yield return new MixedTileContainer(mixedTile, null, MixedTilePartType.Bottom);
                }
                else if (tile is RegularTile regularTile)
                {
                    yield return new RegularTileContainer(regularTile, null);
                }
            }
        }

        protected IEnumerable<MergeContainer> GetSortedTileContainers()
        {
            IEnumerable<ValueTile> allValueTiles = _board.GetAllTiles<ValueTile>();
            IEnumerable<MergeContainer> tiles = GetAllMergeableTileParts(allValueTiles);
            return tiles.OrderByDescending(LargestTileSortingRule).ThenByDescending(AgeTileSortingRule);
        }

        private int LargestTileSortingRule(MergeContainer container)
        {
            return container.GetValue();
        }

        private int AgeTileSortingRule(MergeContainer container)
        {
            return container.Tile.Age;
        }
    }
}