namespace Tiles
{
    public abstract class ValueTile : Tile
    {
        public int Age;

        public abstract bool HasColor(int color);
    }
}