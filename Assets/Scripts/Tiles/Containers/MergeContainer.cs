namespace Tiles.Containers
{
    public abstract class MergeContainer
    {
        public abstract ValueTile Tile { get; }
        public MergeContainer Target;
        public abstract int GetValue();
        public abstract int? GetColor();
        public abstract void IncrementValue(int value = 1);
        public abstract bool IsMergeable(MergeContainer other);

        protected MergeContainer(MergeContainer target)
        {
            Target = target;
        }

        public static MergeContainer GetMergeContainer(ValueTile tile, MergeContainer target)
        {
            if (tile is RegularTile regularTile)
                return new RegularTileContainer(regularTile, target);

            if (tile is MixedTile mixedTile)
            {
                if (target is RegularTileContainer regularMergeContainer)
                {
                    if (mixedTile.Top.Color == regularMergeContainer.GetColor())
                        return new MixedTileContainer(mixedTile, target, MixedTilePartType.Top);
                    if (mixedTile.Bottom.Color == regularMergeContainer.GetColor())
                        return new MixedTileContainer(mixedTile, target, MixedTilePartType.Bottom);

                    return new MixedTileContainer(mixedTile, target, MixedTilePartType.None);
                }

                if (target is MixedTileContainer mixedMergeContainer)
                    return new MixedTileContainer(mixedTile, target, mixedMergeContainer.PartType);
            }

            return null;
        }
    }
}