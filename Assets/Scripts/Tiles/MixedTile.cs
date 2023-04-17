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
        }

        public override bool HasColor(int color)
        {
            return Top.Color == color || Bottom.Color == color;
        }

        public bool TryGetValue(int color, out int value)
        {
            if (Top.Color == color)
            {
                value = Top.Value;
                return true;
            }

            if (Bottom.Color == color)
            {
                value = Bottom.Value;
                return true;
            }

            value = 0;
            return false;
        }
    }
}