using System.Collections.Generic;
using System.Linq;
using GameBoard.Actions;
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

        public override BoardAction GetAction()
        {
            foreach (Tile tile in GetSortedTiles())
            {
                if (!IsValid(tile))
                    continue;

                IEnumerable<ValueTile> validNearbyTiles = GetNearbyTiles(tile.BoardPosition).Where(IsValid).Cast<ValueTile>().ToList();

                if (tile is RegularTile regularTile)
                {
                    ValueTile[] tiles = GetNearbyMergeableTiles(regularTile.Value, regularTile.Color, validNearbyTiles);

                    if (tiles != null)
                        return new MergeTileBoardAction(_board, regularTile, tiles);
                }
                else if (tile is MixedTile mixedTile)
                {
                    ValueTile[] topTiles = GetNearbyMergeableTiles(mixedTile.Top.Value, mixedTile.Top.Color, validNearbyTiles);

                    if (topTiles != null)
                        return new MergeTileBoardAction(_board, mixedTile, topTiles);

                    ValueTile[] bottomTiles = GetNearbyMergeableTiles(mixedTile.Bottom.Value, mixedTile.Bottom.Color, validNearbyTiles);

                    if (bottomTiles != null)
                        return new MergeTileBoardAction(_board, mixedTile, bottomTiles);
                }
            }

            return null;
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

        private IEnumerable<Tile> GetSortedTiles()
        {
            return _board.GetAllTiles().OrderByDescending(tile => tile.Age);
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