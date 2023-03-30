using System;
using System.Collections.Generic;

namespace GameLoop.Rules
{
    [Serializable]
    public class MixedTileRules
    {
        public bool IncludeMixedTiles;
        public List<MixedTileColorCombination> PlayableColorIndexCombinations;
    }
}