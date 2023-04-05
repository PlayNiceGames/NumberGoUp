﻿namespace Tiles
{
    public class EmptyTileData : TileData
    {
        public override TileType Type => TileType.Empty;

        public override bool ContainsColor(int color)
        {
            return false;
        }

        public override bool Equals(TileData other)
        {
            return other != null && other.Type == TileType.Empty;
        }
    }
}