namespace Tiles
{
    public class MixedTileData : TileData
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
    }
}