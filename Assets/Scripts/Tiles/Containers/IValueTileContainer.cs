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
                return new RegularTileContainer(regularTile);

            if (tile is MixedTile mixedTile)
            {
                if (mergeContainer is RegularTileContainer regularMergeContainer)
                {
                    if (mixedTile.Top.Color == regularMergeContainer.GetColor())
                        return new MixedTileContainer(mixedTile, MixedTilePartType.Top);
                    if (mixedTile.Bottom.Color == regularMergeContainer.GetColor())
                        return new MixedTileContainer(mixedTile, MixedTilePartType.Bottom);
                    
                    return new MixedTileContainer(mixedTile, MixedTilePartType.None);
                }

                if (mergeContainer is MixedTileContainer mixedMergeContainer)
                    return new MixedTileContainer(mixedTile, mixedMergeContainer.PartType);
            }

            return null;
        }
    }
}