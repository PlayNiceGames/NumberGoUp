namespace Tiles
{
    public class RegularTileData : ValueTileData
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

        public override bool Equals(TileData other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (other is RegularTileData otherRegularTile)
                return Value == otherRegularTile.Value && Color == otherRegularTile.Color;

            return false;
        }
    }
}