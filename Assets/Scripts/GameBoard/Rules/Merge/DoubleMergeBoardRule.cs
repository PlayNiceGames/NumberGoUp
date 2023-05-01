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
        public DoubleMergeBoardRule(Board board, ScoreSystem scoreSystem) : base(board, scoreSystem)
        {
        }

        public override BoardTurn GetTurn()
        {
            IEnumerable<IValueTileContainer> sortedTiles = GetSortedTileContainers();

            foreach (IValueTileContainer container in sortedTiles)
            {
                IEnumerable<IValueTileContainer> diagonalContainers = GetDiagonalTileContainers(container);

                foreach (IValueTileContainer diagonalContainer in diagonalContainers)
                {
                    List<IValueTileContainer> mergeableContainers = GetMergeableContainers(container, diagonalContainer);

                    if (mergeableContainers == null)
                        continue;

                    DoubleMergeBoardTurn bothPartsTurn = TryGetBothPartsTurn(container, diagonalContainer, mergeableContainers);
                    return bothPartsTurn ?? new DoubleMergeBoardTurn(container, diagonalContainer, mergeableContainers, _board, _scoreSystem);
                }
            }

            return null;
        }

        private DoubleMergeBoardTurn TryGetBothPartsTurn(IValueTileContainer container, IValueTileContainer diagonalContainer, IEnumerable<IValueTileContainer> mergeableContainers)
        {
            if (container is not MixedTileContainer mixedContainer || diagonalContainer is not MixedTileContainer mixedDiagonalContainer)
                return null;

            MixedTileContainer otherPartContainer = mixedContainer.GetOtherPart();
            MixedTileContainer otherDiagonalContainer = mixedDiagonalContainer.GetOtherPart();

            List<IValueTileContainer> otherPartMergeableContainers = GetMergeableContainers(otherPartContainer, otherDiagonalContainer);

            if (otherPartMergeableContainers == null)
                return null;

            MixedTileContainer bothPartsContainer = new MixedTileContainer(mixedContainer.MixedTile, MixedTilePartType.Both);
            MixedTileContainer bothPartsDiagonalContainer = new MixedTileContainer(mixedDiagonalContainer.MixedTile, MixedTilePartType.Both);

            IEnumerable<IValueTileContainer> bothPartsMergeableContainers = mergeableContainers.Select(mergeableContainer =>
                IValueTileContainer.GetMergeContainer(mergeableContainer.Tile, bothPartsContainer));

            return new DoubleMergeBoardTurn(bothPartsContainer, bothPartsDiagonalContainer, bothPartsMergeableContainers, _board, _scoreSystem);
        }

        private List<IValueTileContainer> GetMergeableContainers(IValueTileContainer container, IValueTileContainer diagonalContainer)
        {
            Vector2Int firstPosition = new Vector2Int(container.Tile.BoardPosition.x, diagonalContainer.Tile.BoardPosition.y);
            Vector2Int secondPosition = new Vector2Int(diagonalContainer.Tile.BoardPosition.x, container.Tile.BoardPosition.y);

            if (_board.GetTile(firstPosition) is not ValueTile firstTile || _board.GetTile(secondPosition) is not ValueTile secondTile)
                return null;

            IValueTileContainer firstTileContainer = IValueTileContainer.GetMergeContainer(firstTile, container);
            IValueTileContainer secondTileContainer = IValueTileContainer.GetMergeContainer(secondTile, container);

            if (!firstTileContainer.IsMergeable(container) || !secondTileContainer.IsMergeable(container))
                return null;

            int sum = firstTileContainer.GetValue() + secondTileContainer.GetValue();

            if (sum != container.GetValue())
                return null;

            return new List<IValueTileContainer> {firstTileContainer, secondTileContainer};
        }

        private IEnumerable<IValueTileContainer> GetDiagonalTileContainers(IValueTileContainer container)
        {
            IEnumerable<ValueTile> diagonalTiles = _board.GetDiagonalTiles(container.Tile.BoardPosition).OfType<ValueTile>();
            IEnumerable<ValueTile> sameDiagonalTiles = diagonalTiles.Where(tile => tile.Equals(container.Tile));
            IEnumerable<IValueTileContainer> diagonalContainers = sameDiagonalTiles.Select(sameTile => IValueTileContainer.GetMergeContainer(sameTile, container));

            return diagonalContainers;
        }
    }
}