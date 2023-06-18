using System;

namespace Tiles.Containers
{
    public class MixedTileContainer : MergeContainer
    {
        public override ValueTile Tile => MixedTile;
        public MixedTilePartType PartType { get; }
        public MixedTile MixedTile { get; }

        public MixedTileContainer(MixedTile mixedTile, MergeContainer target, MixedTilePartType partType) : base(target)
        {
            MixedTile = mixedTile;
            PartType = partType;
        }

        public override int GetValue()
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

        public override int? GetColor()
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

        public override void IncrementValue(int value = 1)
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

        public override bool IsMergeable(MergeContainer other)
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

        public MixedTileContainer GetOtherPart()
        {
            MixedTilePartType newPartType;

            switch (PartType)
            {
                case MixedTilePartType.Top:
                    newPartType = MixedTilePartType.Bottom;
                    break;
                case MixedTilePartType.Bottom:
                    newPartType = MixedTilePartType.Top;
                    break;
                case MixedTilePartType.Both:
                    newPartType = MixedTilePartType.None;
                    break;
                case MixedTilePartType.None:
                    newPartType = MixedTilePartType.Both;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new MixedTileContainer(MixedTile, Target, newPartType);
        }
    }
}