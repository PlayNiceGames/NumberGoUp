using System;
using GameLoop.Rules.TileRules;

namespace GameLoop.Rules
{
    [Serializable]
    public class GameRulesSet
    {
        public int RuleApplyStartingScore;
        public int BoardSize;
        public int AvailableColorCount;
        public bool IncludeBigTiles;
        public bool IncludeMixedTiles;
        public RegularTileRulesData RegularTileRules;
        public MixedTileRulesData MixedTileRules;
    }
}