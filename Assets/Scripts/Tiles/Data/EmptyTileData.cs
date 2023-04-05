namespace Tiles
{
    public class EmptyTileData : TileData
    {
        public override TileType Type => TileType.Empty;

        public override bool ContainsColor(int color)
        {
            return false;
        }
    }
}