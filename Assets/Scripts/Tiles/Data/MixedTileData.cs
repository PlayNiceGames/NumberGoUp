namespace Tiles.Data
{
    public class MixedTileData : ValueTileData
    {
        public override TileType Type => TileType.Mixed;

        public int TopValue;
        public int TopColor;

        public int BottomValue;
        public int BottomColor;

        public MixedTileData(int topValue, int topColor, int bottomValue, int bottomColor)
        {
            TopValue = topValue;
            TopColor = topColor;

            BottomValue = bottomValue;
            BottomColor = bottomColor;
        }

        public override bool ContainsColor(int color)
        {
            return TopColor == color || BottomColor == color;
        }

        public override bool Equals(TileData other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (other is MixedTileData otherMixedTile)
                return TopValue == otherMixedTile.TopValue && TopColor == otherMixedTile.TopColor;

            return false;
        }
    }
}