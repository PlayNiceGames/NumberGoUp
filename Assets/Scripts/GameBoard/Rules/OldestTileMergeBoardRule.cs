using System.Collections.Generic;
using System.Linq;
using GameBoard.Actions;
using Tiles;
using UnityEngine;

namespace GameBoard.Rules
{
    public class OldestTileMergeBoardRule : BoardRule
    {
        public OldestTileMergeBoardRule(Board board) : base(board)
        {
        }

        public override BoardAction GetAction()
        {
            BoardAction action = null;

            foreach (Tile tile in GetSortedTiles())
            {
                if (!IsValid(tile))
                    continue;

                IEnumerable<Tile> validNearbyTiles = GetNearbyTiles(tile.BoardPosition).Where(IsValid);

                if (tile is RegularTile regularTile)
                    action = GetRegularTileAction(regularTile, validNearbyTiles);
                else if (tile is MixedTile mixedTile)
                    action = GetMixedTileAction(mixedTile, validNearbyTiles);

                if (action != null)
                    return action;
            }

            return null;
        }

        private BoardAction GetRegularTileAction(RegularTile tile, IEnumerable<Tile> validNearbyTiles) //TODO maybe make separate rules for Regular and Mixed tiles
        {
            IEnumerable<Tile[]> nearbyTilePermutations = 

            int sum = 0;

            foreach (Tile nearbyTile in validNearbyTiles)
            {
                if (nearbyTile is RegularTile regularNearbyTile)
                {
                    if (regularNearbyTile.Color == tile.Color)
                        sum += regularNearbyTile.Value;
                }
            }
        }

        private BoardAction GetMixedTileAction(MixedTile tile, IEnumerable<Tile> validNearbyTiles) //TODO maybe make separate rules for Regular and Mixed tiles
        {
            foreach (Tile nearbyTile in validNearbyTiles)
            {
                if (nearbyTile is RegularTile regularNearbyTile)
                {
                    if (regularNearbyTile.Color == nearbyTile)
                }
            }
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
            return tile != null && tile.Type == TileType.Regular || tile.Type == TileType.Mixed;
        }
    }
}