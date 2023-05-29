namespace Tiles.Data
{
    public class VoidTileData : TileData
    {
        public override TileType Type => TileType.Void;

        public override bool Equals(TileData other)
        {
            return other != null && other.Type == TileType.Void;
        }
    }
}