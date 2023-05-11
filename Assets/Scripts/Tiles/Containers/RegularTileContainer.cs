namespace Tiles.Containers
{
    public class RegularTileContainer : MergeContainer
    {
        public override ValueTile Tile => RegularTile;
        public RegularTile RegularTile { get; private set; }

        public RegularTileContainer(RegularTile regularTile)
        {
            RegularTile = regularTile;
        }

        public override int GetValue()
        {
            return RegularTile.Value;
        }

        public override int? GetColor()
        {
            return RegularTile.Color;
        }

        public override void IncrementValue(int value = 1)
        {
            RegularTile.IncrementValue(value);
        }

        public override bool IsMergeable(MergeContainer other)
        {
            int? otherColor = other.GetColor();
            return otherColor != null && otherColor == GetColor();
        }
    }
}