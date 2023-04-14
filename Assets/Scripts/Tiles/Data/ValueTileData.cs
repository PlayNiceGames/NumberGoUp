namespace Tiles
{
    public abstract class ValueTileData : TileData
    {
        public override TileType Type { get; }

        public abstract bool ContainsColor(int color);
    }
}