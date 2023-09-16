﻿using System.Collections.Generic;
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

        protected IEnumerable<MergeContainer> GetSortedTileContainers()
        {
            IEnumerable<ValueTile> allValueTiles = _board.GetAllTiles<ValueTile>();
            IEnumerable<MergeContainer> tiles = GetAllMergeableTileParts(allValueTiles);
            
            return tiles.OrderByDescending(BothPartsSortingRule).ThenByDescending(LargestTileSortingRule).ThenByDescending(AgeTileSortingRule);
        }

        protected IEnumerable<MergeContainer> GetAllMergeableTileParts(IEnumerable<ValueTile> tiles)
        {
            foreach (ValueTile tile in tiles)
            {
                if (tile is MixedTile mixedTile)
                {
                    yield return new MixedTileContainer(mixedTile, null, MixedTilePartType.Both);
                    yield return new MixedTileContainer(mixedTile, null, MixedTilePartType.Top);
                    yield return new MixedTileContainer(mixedTile, null, MixedTilePartType.Bottom);
                }
                else if (tile is RegularTile regularTile)
                {
                    yield return new RegularTileContainer(regularTile, null);
                }
            }
        }

        private int BothPartsSortingRule(MergeContainer container)
        {
            if (container is MixedTileContainer mixedTileContainer)
            {
                if (mixedTileContainer.PartType == MixedTilePartType.Both)
                    return 1;
            }

            return 0;
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