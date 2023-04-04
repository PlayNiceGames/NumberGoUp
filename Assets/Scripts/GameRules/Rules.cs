using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace GameRules
{
    public class Rules : MonoBehaviour
    {
        [SerializeField] private RulesDatabase _rulesDatabase;
        [SerializeField] private TileColorsDatabase _colorsDatabase;

        public RulesData CurrentRules { get; private set; }

        private List<int> _mixedColors;

        public void Setup()
        {
            CurrentRules = _rulesDatabase.InitialRules;
            _mixedColors = _colorsDatabase.GetRandomColors();
        }

        public int GetRandomTileColor()
        {
            int randomIndex = Random.Range(0, CurrentRules.AvailableColorCount);
            return _mixedColors[randomIndex];
        }

        public (int topColor, int bottomColor) GetRandomMixedTileColors()
        {
            List<MixedTileColorCombination> combinations = CurrentRules.MixedTileRules.ColorIndexCombinations;

            MixedTileColorCombination randomCombination = combinations[Random.Range(0, combinations.Count)];
            return (_mixedColors[randomCombination.TopColorIndex], _mixedColors[randomCombination.BottomColorIndex]);
        }
    }
}