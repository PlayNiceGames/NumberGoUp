using System.Collections.Generic;
using System.Linq;
using GameLoop.Rules.TileRules;
using GameScore;
using Serialization;
using Tiles;
using UnityEngine;
using Utils;

namespace GameLoop.Rules
{
    public class GameRules : MonoBehaviour, IDataSerializable<GameRulesData>
    {
        [SerializeField] private GameRulesDatabase _rulesData;
        [SerializeField] private TileColorsDatabase _colorsData;
        [SerializeField] private ScoreSystem _scoreSystem;

        public GameRulesSet CurrentRules { get; private set; }

        private List<int> _mixedColors;

        public void SetInitialData()
        {
            CurrentRules = _rulesData.GetInitialRules();
            _mixedColors = _colorsData.GetRandomColors();
        }

        public void UpdateCurrentRules()
        {
            int currentScore = _scoreSystem.Score;
            CurrentRules = _rulesData.GetRules(currentScore);

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

        public GameRulesData GetData()
        {
            return new GameRulesData(_mixedColors);
        }

        public void SetData(GameRulesData data)
        {
            _mixedColors = data.MixedColors;
        }
    }
}