using System;

namespace Tiles.Containers
{
    public class MixedTileContainer : IValueTileContainer
    {
        public ValueTile Tile => MixedTile;
        public MixedTilePartType PartType { get; private set; }
        public MixedTile MixedTile { get; private set; }

        public MixedTileContainer(MixedTile mixedTile, MixedTilePartType partType)
        {
            MixedTile = mixedTile;
            PartType = partType;
        }

        public int GetValue()
        {
            return GetValue(PartType);
        }

        public int GetValue(MixedTilePartType partType)
        {
            switch (partType)
            {
                case MixedTilePartType.Top:
                    return MixedTile.Top.Value;
                case MixedTilePartType.Bottom:
                    return MixedTile.Bottom.Value;
                case MixedTilePartType.Both:
                    return Math.Max(MixedTile.Top.Value, MixedTile.Bottom.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int? GetColor()
        {
            return GetColor(PartType);
        }

        public int? GetColor(MixedTilePartType partType)
        {
            switch (partType)
            {
                case MixedTilePartType.Top:
                    return MixedTile.Top.Color;
                case MixedTilePartType.Bottom:
                    return MixedTile.Bottom.Color;
                default:
                    return null;
            }
        }

        public void IncrementValue(int value = 1)
        {
            switch (PartType)
            {
                case MixedTilePartType.Top:
                    MixedTile.Top.IncrementValue(value);
                    break;
                case MixedTilePartType.Bottom:
                    MixedTile.Bottom.IncrementValue(value);
                    break;
                case MixedTilePartType.Both:
                    MixedTile.Top.IncrementValue(value);
                    MixedTile.Bottom.IncrementValue(value);
                    break;
            }
        }

        public bool IsMergeable(IValueTileContainer other)
        {
            if (PartType == MixedTilePartType.None)
                return false;

            if (PartType == MixedTilePartType.Both)
            {
                if (other is MixedTileContainer mixedTileContainer)
                {
                    return mixedTileContainer.GetColor(MixedTilePartType.Top) == GetColor(MixedTilePartType.Top) &&
                           mixedTileContainer.GetColor(MixedTilePartType.Bottom) == GetColor(MixedTilePartType.Bottom);
                }
            }

            return GetColor() == other.GetColor();
        }
    }
}