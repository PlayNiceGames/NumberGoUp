using Tiles.Data;

namespace GameTileQueue.Generators
{
    public abstract class TileQueueGenerator
    {
        public abstract TileData GetNextTileData();
    }
}