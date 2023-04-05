namespace Tiles
{
    public abstract class TileData
    {
        public abstract TileType Type { get; }

        public abstract bool ContainsColor(int color);
    }
}