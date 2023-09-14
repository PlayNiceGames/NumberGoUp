﻿using UnityEngine;

namespace GameTileQueue.Generators
{
    public class TileQueueGeneratorSettings : ScriptableObject
    {
        public int TileQueueSize;

        public int GuaranteedColorTileValue;
        public int GuaranteedColorNotAppearingMaxCount;

        public float BigTileQueueGenerationChance;
        public int BigTileValue;

        public int MixedTileValue;

        public int RemainingTileValue;

        public int MaxRepeatingTileCount;
        public int RepeatingFixTileValue;

        public int EraserTileStartingScore;
        public int EraserTileScoreIncrement;
    }
}