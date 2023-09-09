using Serialization;
using Tiles.Data;

namespace GameTileQueue.Generators
{
    public abstract class TileQueueGenerator : IDataSerializable<TileQueueGeneratorData>
    {
        public abstract TileData GetNextTileData();
        public abstract TileQueueGeneratorData GetData();
        public abstract void SetData(TileQueueGeneratorData data);

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