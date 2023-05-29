using Tiles.Data;

namespace GameTileQueue
{
    public abstract class TileQueueGenerator
    {
        public abstract TileData GetNextTileData();
    }
}