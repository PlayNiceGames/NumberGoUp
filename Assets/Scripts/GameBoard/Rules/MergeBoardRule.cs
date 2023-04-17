using System.Collections.Generic;
using System.Linq;
using GameBoard.Turns;
using GameBoard.Turns.Merge;
using Tiles;
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
            foreach (ValueTile tile in GetSortedTiles())
            {
                List<ValueTile> validNearbyTiles = GetNearbyTiles(tile.BoardPosition).Where(IsValid).Cast<ValueTile>().ToList();

                if (tile is RegularTile regularTile)
                {
                    MergeRegularTileBoardTurn regularTileTurn = GetRegularTileTurn(regularTile, validNearbyTiles);

                    if (regularTileTurn != null)
                        return regularTileTurn;
                }
                else if (tile is MixedTile mixedTile)
                {
                    MergeMixedTileBoardTurn mixedTileTurn = GetMixedTileTurn(mixedTile, validNearbyTiles);

                    if (mixedTileTurn != null)
                        return mixedTileTurn;
                }
            }

            return null;
        }

        private MergeRegularTileBoardTurn GetRegularTileTurn(RegularTile tile, List<ValueTile> validNearbyTiles)
        {
            ValueTile[] tiles = GetNearbyMergeableTiles(tile.Value, tile.Color, validNearbyTiles);

            return tiles != null ? new MergeRegularTileBoardTurn(tile, tiles, _board) : null;
        }

        private MergeMixedTileBoardTurn GetMixedTileTurn(MixedTile tile, List<ValueTile> validNearbyTiles)
        {
            MergeMixedTileBoardTurn topTurn = GetMixedTilePartTurn(tile, tile.Top, validNearbyTiles);

            if (topTurn != null)
                return topTurn;

            MergeMixedTileBoardTurn bottomTurn = GetMixedTilePartTurn(tile, tile.Bottom, validNearbyTiles);
            return bottomTurn;
        }

        private MergeMixedTileBoardTurn GetMixedTilePartTurn(MixedTile tile, MixedTileModel part, List<ValueTile> validNearbyTiles)
        {
            ValueTile[] topTiles = GetNearbyMergeableTiles(part.Value, part.Color, validNearbyTiles);

            return topTiles != null ? new MergeMixedTileBoardTurn(tile, part, topTiles, _board) : null;
        }

        private ValueTile[] GetNearbyMergeableTiles(int value, int color, IEnumerable<ValueTile> validNearbyTiles) //TODO maybe make separate rules for Regular and Mixed tiles
        {
            validNearbyTiles = validNearbyTiles.Where(nearbyTile => nearbyTile.HasColor(color));

            IEnumerable<ValueTile[]> combinations = validNearbyTiles.Combinations();

            foreach (ValueTile[] combination in combinations)
            {
                int sum = GetTilesSumByColor(combination, color);

                if (sum == value)
                    return combination;
            }

            return null;
        }

        private int GetTilesSumByColor(ValueTile[] tiles, int color)
        {
            int sum = 0;

            foreach (ValueTile tile in tiles)
            {
                if (tile is RegularTile regularTile)
                {
                    if (regularTile.Color == color)
                        sum += regularTile.Value;
                }
                else if (tile is MixedTile mixedTile)
                {
                    if (mixedTile.TryGetValue(color, out int value))
                        sum += value;
                }
            }

            return sum;
        }

        private IEnumerable<ValueTile> GetSortedTiles()
        {
            return _board.GetAllTiles<ValueTile>().OrderByDescending(tile => tile.Age);
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