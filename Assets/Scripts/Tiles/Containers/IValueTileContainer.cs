namespace Tiles.Containers
{
    public interface IValueTileContainer
    {
        public ValueTile Tile { get; }
        public int GetValue();
        public int? GetColor();
        public void IncrementValue(int value = 1);
        public bool IsMergeable(IValueTileContainer other);

        public static IValueTileContainer GetMergeContainer(ValueTile tile, IValueTileContainer mergeContainer)
        {
            if (tile is RegularTile regularTile)
            {
                return new RegularTileContainer(regularTile);
            }

            if (tile is MixedTile mixedTile)
            {
                MixedTilePartType partType = MixedTilePartType.None;

                if (mergeContainer is RegularTileContainer regularMergeContainer)
                {
                    if (mixedTile.Top.Color == regularMergeContainer.GetColor())
                        partType = MixedTilePartType.Top;
                    else if (mixedTile.Bottom.Color == regularMergeContainer.GetColor())
                        partType = MixedTilePartType.Bottom;
                }
                else if (mergeContainer is MixedTileContainer mixedMergeContainer)
                {
                    partType = mixedMergeContainer.PartType;
                }

                return new MixedTileContainer(mixedTile, partType);
            }

            return null;
        }
    }
}