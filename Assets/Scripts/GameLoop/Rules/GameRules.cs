using System;

namespace GameLoop.Rules
{
    [Serializable]
    public class GameRules
    {
        public int RuleApplyStartingScore;
        public RegularTileRules RegularTileRules;
        public MixedTileRules MixedTileRules;
    }
}