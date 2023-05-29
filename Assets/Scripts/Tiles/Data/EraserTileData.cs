using System;

namespace Tiles.Data
{
    [Serializable]
    public class EraserTileData : TileData
    {
        public override TileType Type => TileType.Eraser;

        public EraserTileData()
        {
        }

        public override bool Equals(TileData other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return other is EraserTileData;
        }
    }
}