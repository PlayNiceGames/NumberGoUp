using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tiles
{
    public class RegularTile : Tile
    {
        public override TileType Type => TileType.Regular;
        
        public int TileColor { get; private set; }

        [SerializeField] private TextMeshProUGUI _numberText;
        [SerializeField] private Image _background;

        [SerializeField] private TileColorsDatabase _colorsDatabase;

        public void SetNumber(int number)
        {
            _numberText.text = number.ToString();
        }

        public void SetColor(int colorIndex)
        {
            TileColor = colorIndex;

            Color color = _colorsDatabase.GetColor(colorIndex);

            _background.color = color;
        }
    }
}