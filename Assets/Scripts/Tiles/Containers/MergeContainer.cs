namespace Tiles.Containers
{
    public abstract class MergeContainer
    {
        public abstract ValueTile Tile { get; }
        public ValueTile Target { get; }
        public abstract int GetValue();
        public abstract int? GetColor();
        public abstract void IncrementValue(int value = 1);
        public abstract bool IsMergeable(MergeContainer other);

        protected MergeContainer(ValueTile target)
        {
            Target = target;
        }

        public static MergeContainer GetMergeContainer(ValueTile tile, MergeContainer otherMergeContainer)
        {
            ValueTile target = otherMergeContainer.Target;

            if (tile is RegularTile regularTile)
                return new RegularTileContainer(regularTile, target);

            if (tile is MixedTile mixedTile)
            {
                if (otherMergeContainer is RegularTileContainer regularMergeContainer)
                {
                    if (mixedTile.Top.Color == regularMergeContainer.GetColor())
                        return new MixedTileContainer(mixedTile, target, MixedTilePartType.Top);
                    if (mixedTile.Bottom.Color == regularMergeContainer.GetColor())
                        return new MixedTileContainer(mixedTile, target, MixedTilePartType.Bottom);

                    return new MixedTileContainer(mixedTile, target, MixedTilePartType.None);
                }

                if (otherMergeContainer is MixedTileContainer mixedMergeContainer)
                    return new MixedTileContainer(mixedTile, target, mixedMergeContainer.PartType);
            }

            return null;
        }
    }
}