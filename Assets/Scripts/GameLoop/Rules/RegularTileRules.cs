using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace GameLoop.Rules
{
    [Serializable]
    public class RegularTileRules
    {
        public int AvailableColorCount;

        private List<int> _colors;

        public void InitializeColors(List<int> mixedColors)
        {
            _colors = mixedColors;
        }

        public int GetNextTileColor()
        {
            int randomIndex = Random.Range(0, AvailableColorCount);
            return _colors[randomIndex];
        }
    }
}