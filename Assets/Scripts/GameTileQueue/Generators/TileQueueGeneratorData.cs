using System;
using Tiles.Data;

namespace GameTileQueue.Generators
{
    [Serializable]
    public class TileQueueGeneratorData
    {
        public TileData[] GeneratedTileData;
        public TileQueueSetData CurrentSetData;

        public TileQueueGeneratorData(TileData[] generatedTileData, TileQueueSetData currentSetData)
        {
            GeneratedTileData = generatedTileData;
            CurrentSetData = currentSetData;
        }
    }
}