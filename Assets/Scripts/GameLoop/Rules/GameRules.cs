using System.Collections.Generic;
using System.Linq;
using GameLoop.Rules.TileRules;
using GameScore;
using Tiles;
using UnityEngine;
using Utils;

namespace GameLoop.Rules
{
    public class GameRules : MonoBehaviour
    {
        [SerializeField] private GameRulesDatabase _rulesDatabase;
        [SerializeField] private TileColorsDatabase _colorsDatabase;
        [SerializeField] private ScoreSystem _scoreSystem;

        public GameRulesData CurrentRules { get; private set; }

        private List<int> _mixedColors;

        public void Setup()
        {
            CurrentRules = _rulesDatabase.GetInitialRules();
            _mixedColors = _colorsDatabase.GetRandomColors();
        }

        public void UpdateCurrentRules()
        {
            int currentScore = _scoreSystem.Score;
            CurrentRules = _rulesDatabase.GetRules(currentScore);

            Debug.Log($"Set rules for score: {currentScore} : {CurrentRules.RuleApplyStartingScore}");
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