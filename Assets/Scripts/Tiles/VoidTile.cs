using Tiles.Data;

namespace Tiles
{
    public class VoidTile : Tile
    {
        public override TileType Type => TileType.Void;

        public override TileData GetData()
        {
            return new VoidTileData();
        }

        public override bool Equals(Tile other)
        {
            return other != null && other.Type == TileType.Void;
        }
    }
}