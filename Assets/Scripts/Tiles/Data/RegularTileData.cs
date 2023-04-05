namespace Tiles
{
    public class RegularTileData : TileData
    {
        public override TileType Type => TileType.Regular;

        public int Value;
        public int Color;

        public RegularTileData(int value, int color)
        {
            Value = value;
            Color = color;
        }

        public override bool ContainsColor(int color)
        {
            return Color == color;
        }
    }
}