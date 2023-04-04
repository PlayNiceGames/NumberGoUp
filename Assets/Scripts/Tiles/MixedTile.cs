namespace Tiles
{
    public class MixedTile : Tile
    {
        public override TileType Type => TileType.Mixed;

        public int TopValue { get; private set; }
        public int TopColor { get; private set; }

        public int BottomValue { get; private set; }
        public int BottomColor { get; private set; }

        public void Setup(MixedTileData mixedTileData)
        {
            TopValue = mixedTileData.TopValue;
            TopColor = mixedTileData.TopColor;

            BottomValue = mixedTileData.BottomValue;
            BottomColor = mixedTileData.BottomColor;
        }
    }
}