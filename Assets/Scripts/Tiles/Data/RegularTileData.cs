using System;

namespace Tiles.Data
{
    [Serializable]
    public class RegularTileData : ValueTileData
    {
        public override TileType Type => TileType.Regular;

        public int Value;
        public int Color;

        public RegularTileData(int value, int color, int age = 0) : base(age)
        {
            Value = value;
            Color = color;
        }

        public override bool HasColor(int color)
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