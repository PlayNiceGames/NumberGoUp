using System;
using System.Collections.Generic;

namespace GameTileQueue.Generators
{
    [Serializable]
    public class TileQueueSetData
    {
        public bool BigTileGenerated;
        public bool MixedTileGenerated;
        public int? PrevGeneratedEraserTileScore;
        public Dictionary<int, int> ColorsNotAppearingCount;

        public TileQueueSetData(bool bigTileGenerated, bool mixedTileGenerated, int? prevGeneratedEraserTileScore, Dictionary<int, int> colorsNotAppearingCount)
        {
            BigTileGenerated = bigTileGenerated;
            MixedTileGenerated = mixedTileGenerated;
            PrevGeneratedEraserTileScore = prevGeneratedEraserTileScore;
            ColorsNotAppearingCount = colorsNotAppearingCount;
        }
    }
}