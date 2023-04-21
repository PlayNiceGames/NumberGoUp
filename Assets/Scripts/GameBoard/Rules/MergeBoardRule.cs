using System.Collections.Generic;
using System.Linq;
using GameBoard.Turns;
using GameBoard.Turns.Merge;
using Tiles;
using Tiles.Containers;
using UnityEngine;
using Utils;

namespace GameBoard.Rules
{
    public class MergeBoardRule : BoardRule
    {
        public MergeBoardRule(Board board) : base(board)
        {
        }

        public override BoardTurn GetTurn()
        {
            IEnumerable<IValueTileContainer> sortedTiles = GetSortedTiles();

            foreach (IValueTileContainer container in sortedTiles)
            {
                List<ValueTile> validNearbyTiles = GetNearbyTiles(container.Tile.BoardPosition).Where(IsValid).Cast<ValueTile>().ToList();
                IValueTileContainer[] tiles = GetNearbyMergeableTiles(container, validNearbyTiles);

                if (tiles == null)
                    continue;

                if (container is RegularTileContainer regularTileContainer)
                {
                    MergeRegularTileBoardTurn regularTileTurn = new MergeRegularTileBoardTurn(regularTileContainer.RegularTile, tiles, _board);
                    return regularTileTurn;
                }

                if (container is MixedTileContainer mixedTileContainer)
                {
                    MergeMixedTileBoardTurn mixedTileTurn = new MergeMixedTileBoardTurn(mixedTileContainer, tiles, _board);
                    return mixedTileTurn;
                }
            }

            return null;
        }

        private IValueTileContainer[] GetNearbyMergeableTiles(IValueTileContainer container, List<ValueTile> validNearbyTiles)
        {
            IEnumerable<IValueTileContainer> validNearbyTileContainers = validNearbyTiles.Select(tile => IValueTileContainer.GetMergeContainer(tile, container));
            IEnumerable<IValueTileContainer> mergeableTiles = validNearbyTileContainers.Where(tile => tile.IsMergeable(container));
            IEnumerable<IValueTileContainer[]> combinations = mergeableTiles.Combinations();

            foreach (IValueTileContainer[] combination in combinations)
            {
                if (container is MixedTileContainer mixedTileContainer)
                {
                    if (mixedTileContainer.PartType == MixedTilePartType.Both)
                    {
                        IEnumerable<MixedTileContainer> mixedTileContainers = combination.Select(combinationContainer => combinationContainer as MixedTileContainer).ToList();
                        int topSum = mixedTileContainers.Sum(combinationContainer => combinationContainer.GetValue(MixedTilePartType.Top));
                        int bottomSum = mixedTileContainers.Sum(combinationContainer => combinationContainer.GetValue(MixedTilePartType.Bottom));

                        if (topSum == mixedTileContainer.GetValue(MixedTilePartType.Top) && bottomSum == mixedTileContainer.GetValue(MixedTilePartType.Bottom))
                            return combination;
                    }
                }

                int sum = combination.Sum(combinationContainer => combinationContainer.GetValue());
                if (sum == container.GetValue())
                    return combination;
            }

            return null;
        }

        private IEnumerable<IValueTileContainer> GetSortedTiles()
        {
            IEnumerable<IValueTileContainer> tiles = GetAllMergeableTileParts();
            return tiles.OrderByDescending(LargestTileSortingRule).ThenByDescending(AgeTileSortingRule);
        }

        private IEnumerable<IValueTileContainer> GetAllMergeableTileParts()
        {
            IEnumerable<ValueTile> allMergeableTiles = _board.GetAllTiles<ValueTile>();

            foreach (ValueTile tile in allMergeableTiles)
            {
                if (tile is MixedTile mixedTile)
                {
                    yield return new MixedTileContainer(mixedTile, MixedTilePartType.Both);
                    yield return new MixedTileContainer(mixedTile, MixedTilePartType.Top);
                    yield return new MixedTileContainer(mixedTile, MixedTilePartType.Bottom);
                }
                else if (tile is RegularTile regularTile)
                {
                    yield return new RegularTileContainer(regularTile);
                }
            }
        }

        private int LargestTileSortingRule(IValueTileContainer container)
        {
            return container.GetValue();
        }

        private int AgeTileSortingRule(IValueTileContainer container)
        {
            return container.Tile.Age;
        }

        public IEnumerable<Tile> GetNearbyTiles(Vector2Int position)
        {
            yield return _board.GetTile(new Vector2Int(position.x - 1, position.y));
            yield return _board.GetTile(new Vector2Int(position.x + 1, position.y));
            yield return _board.GetTile(new Vector2Int(position.x, position.y - 1));
            yield return _board.GetTile(new Vector2Int(position.x, position.y + 1));
        }

        private bool IsValid(Tile tile)
        {
            return tile != null && tile is ValueTile;
        }
    }
}