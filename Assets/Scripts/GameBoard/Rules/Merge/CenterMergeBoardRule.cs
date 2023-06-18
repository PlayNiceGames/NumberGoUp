using System.Collections.Generic;
using System.Linq;
using GameBoard.Turns;
using GameBoard.Turns.Merge;
using GameScore;
using Tiles;
using Tiles.Containers;

namespace GameBoard.Rules.Merge
{
    public class CenterMergeBoardRule : MergeBoardRule
    {
        public CenterMergeBoardRule(Board board, ScoreSystem scoreSystem) : base(board, scoreSystem)
        {
            _scoreSystem = scoreSystem;
        }

        public override BoardTurn GetTurn()
        {
            IEnumerable<ValueTile> allTiles = _board.GetAllTiles<ValueTile>();
            foreach (ValueTile tile in allTiles)
            {
                IEnumerable<ValueTile> nearbyTiles = _board.GetNearbyTiles(tile.BoardPosition).OfType<ValueTile>();
                IEnumerable<ValueTile> sameTiles = nearbyTiles.Where(nearbyTile => nearbyTile.Equals(tile)).ToList();

                MergeContainer tileContainer = GetContainer(tile);
                List<MergeContainer> sameTileContainers = sameTiles.Select(sameTile => MergeContainer.GetMergeContainer(sameTile, tileContainer)).ToList();

                int sameTilesCount = sameTileContainers.Count;
                if (sameTilesCount > 1)
                    return new CenterMergeBoardTurn(tileContainer, sameTileContainers, _board, _scoreSystem);
            }

            return null;
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