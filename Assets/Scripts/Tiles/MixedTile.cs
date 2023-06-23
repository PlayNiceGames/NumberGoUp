using System;
using Cysharp.Threading.Tasks;
using Tiles.Data;
using UnityEngine;

namespace Tiles
{
    public class MixedTile : ValueTile
    {
        public override TileType Type => TileType.Mixed;

        [field: SerializeField] public MixedTileModel Top { get; private set; }
        [field: SerializeField] public MixedTileModel Bottom { get; private set; }

        public void Setup(MixedTileData mixedTileData)
        {
            Top.Setup(mixedTileData.TopValue, mixedTileData.TopColor);
            Bottom.Setup(mixedTileData.BottomValue, mixedTileData.BottomColor);

            Age = mixedTileData.Age;
        }

        public override bool HasColor(int color)
        {
            return Top.Color == color || Bottom.Color == color;
        }

        public override TileData GetData()
        {
            return new MixedTileData(Top.Value, Top.Color, Bottom.Value, Bottom.Color);
        }

        public UniTask PlayMergePartAnimation(MixedTilePartType partType, Vector2 position)
        {
            MixedTileModel part = GetPart(partType);
            MixedTileModel otherPart = GetOtherPart(partType);

            UniTask mergeAnimation = part.PlayMergeAnimation(position);
            UniTask scaleAnimation = otherPart.PlayScaleUpAnimation();

            return UniTask.WhenAll(mergeAnimation, scaleAnimation);
        }

        public override bool Equals(Tile other)
        {
            if (other is MixedTile mixedTile)
                return mixedTile.Top.Equals(Top) && mixedTile.Bottom.Equals(Bottom);

            return false;
        }

        public MixedTileModel GetPart(MixedTilePartType partType)
        {
            switch (partType)
            {
                case MixedTilePartType.Top:
                    return Top;
                case MixedTilePartType.Bottom:
                    return Bottom;
                default:
                    throw new ArgumentOutOfRangeException(nameof(partType), partType, null);
            }
        }

        public MixedTileModel GetOtherPart(MixedTilePartType partType)
        {
            switch (partType)
            {
                case MixedTilePartType.Top:
                    return Bottom;
                case MixedTilePartType.Bottom:
                    return Top;
                default:
                    throw new ArgumentOutOfRangeException(nameof(partType), partType, null);
            }
        }
    }
}