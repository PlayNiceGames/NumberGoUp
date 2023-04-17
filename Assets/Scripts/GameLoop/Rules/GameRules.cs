using System.Collections.Generic;
using System.Linq;
using GameLoop.Rules.TileRules;
using Tiles;
using UnityEngine;
using Utils;

namespace GameLoop.Rules
{
    public class GameRules : MonoBehaviour
    {
        [SerializeField] private GameRulesDatabase _rulesDatabase;
        [SerializeField] private TileColorsDatabase _colorsDatabase;

        public GameRulesData CurrentRules { get; private set; }

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

        public int GetColor(int gameColorIndex)
        {
            return _mixedColors[gameColorIndex];
        }
        
        public int GetRandomTileColor()
        {
            return _mixedColors[RandomColorIndex()];
        }

        public int GetRandomTileColorExcept(int color)
        {
            List<int> colors = _mixedColors.Take(CurrentRules.AvailableColorCount).Where(mixedColor => mixedColor != color).ToList();
            return colors.RandomItem();
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