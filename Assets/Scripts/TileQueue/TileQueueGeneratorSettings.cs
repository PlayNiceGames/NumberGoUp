using UnityEngine;

namespace GameTileQueue
{
    public class TileQueueGeneratorSettings : ScriptableObject
    {
        public int TileQueueSize;

        public int GuaranteedColorTileValue;

        public float BigTileQueueGenerationChance;
        public int BigTileValue;

        public float MixedTileQueueGenerationChance;
        public int MixedTileValue;

        public int RemainingTileValue;
        
        public int MaxRepeatingTileCount;
        public int RepeatingFixTileValue;
    }
}