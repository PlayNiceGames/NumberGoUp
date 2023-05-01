using System;
using System.Collections.Generic;

namespace GameLoop.Rules.TileRules
{
    [Serializable]
    public class MixedTileRulesData
    {
        public float QueueGenerationChance;
        public List<MixedTileColorCombination> ColorIndexCombinations;
    }
}