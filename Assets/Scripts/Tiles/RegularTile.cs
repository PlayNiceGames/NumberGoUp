using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tiles
{
    public class RegularTile : Tile
    {
        public override TileType Type => TileType.Regular;
        
        public int Color { get; private set; }

        [SerializeField] private TextMeshProUGUI _numberText;
        [SerializeField] private Image _background;

        [SerializeField] private TileColorsDatabase _colorsDatabase;

        [Button, DisableInEditorMode]
        public void SetNumber(int number)
        {
            _numberText.text = number.ToString();
        }

        [Button, DisableInEditorMode]
        public void SetColor(int colorIndex)
        {
            Color = colorIndex;

            Color color = _colorsDatabase.GetColor(colorIndex);

            _background.color = color;
        }
    }
}