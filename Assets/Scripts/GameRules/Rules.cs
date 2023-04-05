using System.Collections.Generic;
using System.Linq;
using Tiles;
using UnityEngine;
using Utils;

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

        public List<int> GetAvailableColors()
        {
            return _mixedColors.Take(CurrentRules.AvailableColorCount).ToList();
        }

        public int GetRandomTileColor()
        {
            return _mixedColors[RandomColorIndex()];
        }

        public int GetRandomTileColorExcept(int color)
        {
            return _mixedColors.Where(mixedColor => mixedColor != color).ToList()[RandomColorIndex()];
        }

        public (int topColor, int bottomColor) GetRandomMixedTileColors()
        {
            List<MixedTileColorCombination> combinations = CurrentRules.MixedTileRules.ColorIndexCombinations;
            MixedTileColorCombination randomCombination = combinations.RandomItem();

            return (_mixedColors[randomCombination.TopColorIndex], _mixedColors[randomCombination.BottomColorIndex]);
        }

        private int RandomColorIndex()
        {
            return Random.Range(0, CurrentRules.AvailableColorCount);
        }
    }
}