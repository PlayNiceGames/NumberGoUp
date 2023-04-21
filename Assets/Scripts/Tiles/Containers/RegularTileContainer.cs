namespace Tiles.Containers
{
    public class RegularTileContainer : IValueTileContainer
    {
        public ValueTile Tile => RegularTile;
        public RegularTile RegularTile { get; private set; }

        public RegularTileContainer(RegularTile regularTile)
        {
            RegularTile = regularTile;
        }

        public int GetValue()
        {
            return RegularTile.Value;
        }

        public int? GetColor()
        {
            return RegularTile.Color;
        }

        public void IncrementValue(int value)
        {
            RegularTile.IncrementValue(value);
        }

        public bool IsMergeable(IValueTileContainer other)
        {
            int? otherColor = other.GetColor();
            return otherColor != null && otherColor == GetColor();
        }
    }
}