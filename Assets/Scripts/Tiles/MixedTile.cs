using System;
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
        }

        public override bool HasColor(int color)
        {
            return Top.Color == color || Bottom.Color == color;
        }

        /*public bool TryGetPart(int color, out MixedTileModel part)
        {
            if (Top.Color == color)
            {
                part = Top;
                return true;
            }

            if (Bottom.Color == color)
            {
                part = Bottom;
                return true;
            }

            part = null;
            return false;
        }*/
    }
}