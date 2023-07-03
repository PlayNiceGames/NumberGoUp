using Tiles.Data;

namespace GameTileQueue.Generators
{
    public abstract class TileQueueGenerator
    {
        public abstract TileData GetNextTileData();

        public TileData[] GetNextTilesData(int count)
        {
            TileData[] tiles = new TileData[count];

            for (int i = 0; i < count; i++)
            {
                tiles[i] = GetNextTileData();
            }

            return tiles;
        }
    }
}