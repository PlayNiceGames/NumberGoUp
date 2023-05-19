namespace Tiles.Data
{
    public abstract class ValueTileData : TileData
    {
        public int Age;

        public abstract bool HasColor(int color);

        protected ValueTileData(int age)
        {
            Age = age;
        }
    }
}