using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace GameLoop.GameRules
{
    public class Rules : MonoBehaviour
    {
        [SerializeField] private RulesSetDatabase _rulesDatabase;
        [SerializeField] private TileColorsDatabase _colorsDatabase;

        public RulesSet CurrentRules { get; private set; }

        public List<int> MixedColors;

        public void Setup()
        {
            CurrentRules = _rulesDatabase.InitialRules;
            MixedColors = _colorsDatabase.GetRandomColors();
        }
    }
}