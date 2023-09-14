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

        public static MergeContainer TryCreateMergeContainer(ValueTile tile, MergeContainer target)
        {
            MergeContainer container = null;

            if (tile is RegularTile regularTile)
            {
                container = new RegularTileContainer(regularTile, target);
            }
            else if (tile is MixedTile mixedTile)
            {
                if (target is RegularTileContainer regularTileTargetContainer)
                {
                    if (mixedTile.Top.Color == regularTileTargetContainer.GetColor())
                        container = new MixedTileContainer(mixedTile, target, MixedTilePartType.Top);
                    if (mixedTile.Bottom.Color == regularTileTargetContainer.GetColor())
                        container = new MixedTileContainer(mixedTile, target, MixedTilePartType.Bottom);
                }
                else if (target is MixedTileContainer mixedTileTargetContainer)
                {
                    container = new MixedTileContainer(mixedTile, target, mixedTileTargetContainer.PartType);
                }
            }

            if (container != null && container.IsMergeable(target))
                return container;

            return null;
        }
    }
}