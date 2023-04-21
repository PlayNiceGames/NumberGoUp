namespace Tiles
{
    public class EmptyTile : Tile
    {
        public override TileType Type => TileType.Empty;

        public override bool Equals(Tile other)
        {
            return other is EmptyTile;
        }
    }
}