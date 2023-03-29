using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tile
{
    public class RegularTile : TileBase
    {
        public int TileColor { get; private set; }

        [SerializeField] private TextMeshProUGUI _numberText;
        [SerializeField] private Image _background;

        [SerializeField] private TileColors _colorsDatabase;

        private void Awake()
        {
            SetNumber(Random.Range(0, 1000));
            SetColor(_colorsDatabase.GetRandomColorIndex());
        }

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