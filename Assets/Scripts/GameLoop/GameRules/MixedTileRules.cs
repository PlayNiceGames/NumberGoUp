using System;
using System.Collections.Generic;

namespace GameLoop.GameRules
{
    [Serializable]
    public class MixedTileRules
    {
        public bool IncludeMixedTiles;
        public List<MixedTileColorCombination> PlayableColorIndexCombinations;
        
        private List<int> _colors;

        public void Setup(List<int> mixedColors)
        {
            _colors = mixedColors;
        }
    }
}