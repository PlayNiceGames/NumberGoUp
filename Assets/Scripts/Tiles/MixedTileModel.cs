using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Tiles.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tiles
{
    public class MixedTileModel : MonoBehaviour, IEquatable<MixedTileModel> //TODO DRY with RegularTile. Maybe create abstract tile model?
    {
        public int Value { get; private set; }
        public int Color { get; private set; }

        [SerializeField] private TextMeshProUGUI _numberText;
        [SerializeField] private Image _background;
        [SerializeField] private TileColorsData _colorsData;
        [Space]
        [SerializeField] private TileMergeAnimation _mergeAnimation;
        [SerializeField] private TilePartScaleAnimation _scaleAnimation;

        public void Setup(int value, int color)
        {
            SetValue(value);
            SetColor(color);
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

            Color color = _colorsData.GetColor(colorIndex);

            _background.color = color;
        }

        public UniTask PlayMergeAnimation(Vector2 position)
        {
            return _mergeAnimation.Play(position);
        }

        public UniTask PlayScaleUpAnimation()
        {
            return _scaleAnimation.Play();
        }

        public bool Equals(MixedTileModel other)
        {
            return other != null && other.Value == Value && other.Color == Color;
        }
    }
}