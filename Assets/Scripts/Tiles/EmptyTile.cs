using Tiles.Data;

namespace Tiles
{
    public class EmptyTile : Tile
    {
        public override TileType Type => TileType.Empty;

        public override TileData GetData()
        {
            return new EmptyTileData();
        }

        public override bool Equals(Tile other)
        {
            return other is EmptyTile;
        }
    }
}