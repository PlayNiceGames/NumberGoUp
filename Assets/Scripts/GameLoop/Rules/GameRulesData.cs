﻿using System;
using GameLoop.Rules.TileRules;

namespace GameLoop.Rules
{
    [Serializable]
    public class GameRulesData
    {
        public int RuleApplyStartingScore;
        public int AvailableColorCount;
        public bool IncludeBigTiles;
        public bool IncludeMixedTiles;
        public RegularTileRulesData RegularTileRules;
        public MixedTileRulesData MixedTileRules;
    }
}