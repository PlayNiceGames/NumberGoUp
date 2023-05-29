using Tiles.Data;

namespace Tiles
{
    public class EraserTile : Tile
    {
        public override TileType Type => TileType.Eraser;

        public override TileData GetData()
        {
            return new EraserTileData();
        }

        public override bool Equals(Tile other)
        {
            return other is EraserTile;
        }
    }
}