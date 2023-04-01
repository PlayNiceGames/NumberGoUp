using System;
using System.Collections.Generic;

namespace GameLoop.GameRules
{
    [Serializable]
    public class RulesSet
    {
        public int RuleApplyStartingScore;
        public RegularTileRules RegularTileRules;
        public MixedTileRules MixedTileRules;

        public void Setup(List<int> mixedColors)
        {
            RegularTileRules.Setup(mixedColors);
            MixedTileRules.Setup(mixedColors);
        }
    }
}