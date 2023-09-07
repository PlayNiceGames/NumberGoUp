using GameLoop.Rules;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Tiles;
using Tiles.Data;
using UnityEngine;

namespace GameDebug
{
    public class DebugTilePlacer : MonoBehaviour
    {
        [field: SerializeField] public bool DebugPlaceTiles { get; private set; }

        [SerializeField] private TileType _type;

        [Space]
        [SerializeField] private int _firstValue;

        [SerializeField] private int _secondValue;

        [Space]
        [SerializeField] private bool _useGameColors;

        [Space]
        [SerializeField] private int _firstColor;

        [SerializeField] private int _secondColor;

        [Space]
        [SerializeField] [ReadOnly] [UsedImplicitly]
        private Color _firstDisplayedColor;

        [SerializeField] [ReadOnly] [UsedImplicitly]
        private Color _secondDisplayedColor;

        [Space]
        [SerializeField] private TileFactory _factory;

        [SerializeField] private GameRules _rules;
        [SerializeField] private TileColorsDatabase _colorsData;

        private bool _isSetup;

        private void Start()
        {
            _isSetup = true;

            UpdateColors();
        }

        private void OnValidate()
        {
            if (!Application.isPlaying || !_isSetup)
                return;

            UpdateColors();
        }

        private void UpdateColors()
        {
            _firstDisplayedColor = GetColor(_firstColor);
            _secondDisplayedColor = GetColor(_secondColor);
        }

        private Color GetColor(int colorIndex)
        {
            int actualColorIndex = GetActualColorIndex(colorIndex);
            return _colorsData.GetColor(actualColorIndex);
        }

        private int GetActualColorIndex(int colorIndex)
        {
            return _useGameColors ? _rules.GetColor(colorIndex) : colorIndex;
        }

        public Tile PopNextTile()
        {
            TileData data;

            switch (_type)
            {
                case TileType.Empty:
                    data = new EmptyTileData();
                    break;
                case TileType.Regular:
                    int colorIndex = GetActualColorIndex(_firstColor);
                    data = new RegularTileData(_firstValue, colorIndex);
                    break;
                case TileType.Mixed:
                    int topColorIndex = GetActualColorIndex(_firstColor);
                    int bottomColorIndex = GetActualColorIndex(_secondColor);
                    data = new MixedTileData(_firstValue, topColorIndex, _secondValue, bottomColorIndex);
                    break;
                default:
                    Tile defaultTile = _factory.InstantiateTile(_type);
                    return defaultTile;
            }

            Tile tile = _factory.InstantiateTile(data);
            return tile;
        }

        public TileType PeekNextTileType()
        {
            return _type;
        }
    }
}