﻿using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tiles
{
    public class RegularTile : ValueTile
    {
        public override TileType Type => TileType.Regular;

        public int Value { get; private set; }
        public int Color { get; private set; }

        [SerializeField] private TextMeshProUGUI _numberText;
        [SerializeField] private Image _background;

        [SerializeField] private TileColorsDatabase _colorsDatabase;

        public void Setup(RegularTileData data)
        {
            SetValue(data.Value);
            SetColor(data.Color);
        }

        public void IncrementValue(int increment = 1)
        {
            SetValue(Value + increment);
        }

        [Button, DisableInEditorMode]
        public void SetValue(int value)
        {
            Value = value;

            _numberText.text = value.ToString();
        }

        [Button, DisableInEditorMode]
        public void SetColor(int colorIndex)
        {
            Color = colorIndex;

            Color color = _colorsDatabase.GetColor(colorIndex);

            _background.color = color;
        }
        
        public override bool HasColor(int color)
        {
            return Color == color;
        }
    }
}