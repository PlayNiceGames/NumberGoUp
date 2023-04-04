namespace Tiles
{
    public class MixedTile : Tile
    {
        public override TileType Type => TileType.Mixed;
        
        public int TopColor { get; private set; }
        public int BottomColor { get; private set; }
    }
}