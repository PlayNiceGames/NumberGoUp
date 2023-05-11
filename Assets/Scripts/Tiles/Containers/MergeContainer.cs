namespace Tiles.Containers
{
    public abstract class MergeContainer
    {
        public abstract ValueTile Tile { get; }
        public abstract int GetValue();
        public abstract int? GetColor();
        public abstract void IncrementValue(int value = 1);
        public abstract bool IsMergeable(MergeContainer other);

        public static MergeContainer GetMergeContainer(ValueTile tile, MergeContainer otherMergeContainer)
        {
            if (tile is RegularTile regularTile)
                return new RegularTileContainer(regularTile);

            if (tile is MixedTile mixedTile)
            {
                if (otherMergeContainer is RegularTileContainer regularMergeContainer)
                {
                    if (mixedTile.Top.Color == regularMergeContainer.GetColor())
                        return new MixedTileContainer(mixedTile, MixedTilePartType.Top);
                    if (mixedTile.Bottom.Color == regularMergeContainer.GetColor())
                        return new MixedTileContainer(mixedTile, MixedTilePartType.Bottom);

                    return new MixedTileContainer(mixedTile, MixedTilePartType.None);
                }

                if (otherMergeContainer is MixedTileContainer mixedMergeContainer)
                    return new MixedTileContainer(mixedTile, mixedMergeContainer.PartType);
            }

            return null;
        }
    }
}