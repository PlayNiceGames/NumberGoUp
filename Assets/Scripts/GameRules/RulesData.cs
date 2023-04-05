using System;

namespace GameRules
{
    [Serializable]
    public class RulesData
    {
        public int RuleApplyStartingScore;
        public int AvailableColorCount;
        public bool IncludeBigTiles;
        public bool IncludeMixedTiles;
        public RegularTileRulesData RegularTileRules;
        public MixedTileRulesData MixedTileRules;
    }
}