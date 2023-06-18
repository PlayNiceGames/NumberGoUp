using System.Collections.Generic;
using System.Linq;
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
        private TileFactory _factory;

        public DoubleMergeBoardRule(Board board, TileFactory factory, ScoreSystem scoreSystem) : base(board, scoreSystem)
        {
            _factory = factory;
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

                    DoubleMergeBoardTurn bothPartsTurn = TryGetBothPartsTurn(container, diagonalContainer, mergeableContainers);
                    return bothPartsTurn ?? new DoubleMergeBoardTurn(container, diagonalContainer, mergeableContainers, _board, _factory, _scoreSystem);
                }
            }

            return null;
        }

        private DoubleMergeBoardTurn TryGetBothPartsTurn(MergeContainer container, MergeContainer diagonalContainer, IEnumerable<MergeContainer> mergeableContainers)
        {
            if (container is not MixedTileContainer mixedContainer || diagonalContainer is not MixedTileContainer mixedDiagonalContainer)
                return null;

            MixedTileContainer otherPartContainer = mixedContainer.GetOtherPart();
            MixedTileContainer otherDiagonalContainer = mixedDiagonalContainer.GetOtherPart();

            List<MergeContainer> otherPartMergeableContainers = GetMergeableContainers(otherPartContainer, otherDiagonalContainer);

            if (otherPartMergeableContainers == null)
                return null;

            MixedTile tile = mixedContainer.MixedTile;
            MixedTileContainer bothPartsContainer = new MixedTileContainer(tile, null, MixedTilePartType.Both);
            MixedTileContainer bothPartsDiagonalContainer = new MixedTileContainer(tile, null, MixedTilePartType.Both);

            IEnumerable<MergeContainer> bothPartsMergeableContainers = mergeableContainers.Select(mergeableContainer =>
                MergeContainer.GetMergeContainer(mergeableContainer.Tile, bothPartsContainer));

            return new DoubleMergeBoardTurn(bothPartsContainer, bothPartsDiagonalContainer, bothPartsMergeableContainers, _board, _factory, _scoreSystem);
        }

        private List<MergeContainer> GetMergeableContainers(MergeContainer container, MergeContainer diagonalContainer)
        {
            Vector2Int firstPosition = new Vector2Int(container.Tile.BoardPosition.x, diagonalContainer.Tile.BoardPosition.y);
            Vector2Int secondPosition = new Vector2Int(diagonalContainer.Tile.BoardPosition.x, container.Tile.BoardPosition.y);

            if (_board.GetTile(firstPosition) is not ValueTile firstTile || _board.GetTile(secondPosition) is not ValueTile secondTile)
                return null;

            MergeContainer firstTileContainer = MergeContainer.GetMergeContainer(firstTile, container);
            MergeContainer secondTileContainer = MergeContainer.GetMergeContainer(secondTile, container);

            if (!firstTileContainer.IsMergeable(container) || !secondTileContainer.IsMergeable(container))
                return null;

            int sum = firstTileContainer.GetValue() + secondTileContainer.GetValue();

            if (sum != container.GetValue())
                return null;

            return new List<MergeContainer> {firstTileContainer, secondTileContainer};
        }

        private IEnumerable<MergeContainer> GetDiagonalTileContainers(MergeContainer container)
        {
            IEnumerable<ValueTile> diagonalTiles = _board.GetDiagonalTiles(container.Tile.BoardPosition).OfType<ValueTile>();
            IEnumerable<ValueTile> sameDiagonalTiles = diagonalTiles.Where(tile => tile.Equals(container.Tile));
            IEnumerable<MergeContainer> diagonalContainers = sameDiagonalTiles.Select(sameTile => MergeContainer.GetMergeContainer(sameTile, container));

            return diagonalContainers;
        }
    }
}