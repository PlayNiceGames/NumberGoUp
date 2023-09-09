using System.Collections.Generic;

namespace GameLoop.Rules
{
    public class GameRulesData
    {
        public List<int> MixedColors;

        public GameRulesData(List<int> mixedColors)
        {
            MixedColors = mixedColors;
        }
    }
}