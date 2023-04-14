using System;

namespace Tiles
{
    public abstract class TileData : IEquatable<TileData>
    {
        public abstract TileType Type { get; }

        public abstract bool Equals(TileData other);
    }
}