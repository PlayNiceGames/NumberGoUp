using System.Collections.Generic;
using System.Linq;
using Analytics;
using GameBoard.Turns;
using GameBoard.Turns.Merge;
using GameScore;
using Tiles;
using Tiles.Containers;
using UnityEngine;

namespace GameBoard.Rules.Merge
{
    public class DoubleMergeBoardRule : MergeBoardRule
    {
        private readonly TileFactory _factory;
        private readonly AnalyticsService _analytics;

        public DoubleMergeBoardRule(Board board, TileFactory factory, ScoreSystem scoreSystem, AnalyticsService analytics) : base(board, scoreSystem)
        {
            _factory = factory;
            _analytics = analytics;
        }

        public override BoardTurn GetTurn()
        {
            IEnumerable<MergeContainer> sortedTiles = GetSortedTileContainers();

            foreach (MergeContainer container in sortedTiles)
            {
                IEnumerable<MergeContainer> diagonalContainers = GetDiagonalTileContainers(container);

                foreach (MergeContainer diagonalContainer in diagonalContainers)
                {
                    List<MergeContainer> mergeableContainers = GetMergeableContainers(container, diagonalContainer);

                    if (mergeableContainers == null)
                        continue;

                    return new DoubleMergeBoardTurn(container, diagonalContainer, mergeableContainers, _board, _factory, _scoreSystem, _analytics);
                }
            }

            return null;
        }

        private List<MergeContainer> GetMergeableContainers(MergeContainer container, MergeContainer diagonalContainer)
        {
            Vector2Int firstPosition = new Vector2Int(container.Tile.BoardPosition.x, diagonalContainer.Tile.BoardPosition.y);
            Vector2Int secondPosition = new Vector2Int(diagonalContainer.Tile.BoardPosition.x, container.Tile.BoardPosition.y);

            if (_board.GetTile(firstPosition) is not ValueTile firstTile || _board.GetTile(secondPosition) is not ValueTile secondTile)
                return null;

            MergeContainer firstTileContainer = MergeContainer.TryCreateMergeContainer(firstTile, container);
            MergeContainer secondTileContainer = MergeContainer.TryCreateMergeContainer(secondTile, container);

            if (firstTileContainer == null || secondTileContainer == null)
                return null;

            int sum = firstTileContainer.GetValue() + secondTileContainer.GetValue();

            if (sum != container.GetValue())
                return null;

            return new List<MergeContainer> { firstTileContainer, secondTileContainer };
        }

        private IEnumerable<MergeContainer> GetDiagonalTileContainers(MergeContainer target)
        {
            IEnumerable<ValueTile> diagonalTiles = _board.GetDiagonalTiles(target.Tile.BoardPosition).OfType<ValueTile>();
            IEnumerable<ValueTile> sameDiagonalTiles = diagonalTiles.Where(tile => tile.Equals(target.Tile));
            IEnumerable<MergeContainer> diagonalContainers = sameDiagonalTiles.Select(sameTile => MergeContainer.TryCreateMergeContainer(sameTile, target))
                .Where(container => container != null);

            return diagonalContainers;
        }
    }
}