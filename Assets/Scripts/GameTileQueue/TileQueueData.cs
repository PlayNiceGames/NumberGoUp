using System;
using GameTileQueue.Generators;
using Tiles.Data;

namespace GameTileQueue
{
    [Serializable]
    public class TileQueueData
    {
        public TileData[] Tiles;
        public TileQueueGeneratorData GeneratorData;

        public TileQueueData(TileData[] tiles, TileQueueGeneratorData generatorData)
        {
            Tiles = tiles;
            GeneratorData = generatorData;
        }
    }
}