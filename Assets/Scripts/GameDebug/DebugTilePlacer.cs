﻿using GameLoop.Rules;
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
        [SerializeField] private int _firstGameColor;
        [SerializeField] private int _secondGameColor;

        [Space]
        [SerializeField] [ReadOnly] private Color _firstDisplayedColor;
        [SerializeField] [ReadOnly] private Color _secondDisplayedColor;

        [Space]
        [SerializeField] private TileFactory _factory;
        [SerializeField] private GameRules _rules;
        [SerializeField] private TileColorsData _colorsData;

        private bool _isSetup;

        public void Setup()
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
            _firstDisplayedColor = GetColor(_firstGameColor);
            _secondDisplayedColor = GetColor(_secondGameColor);
        }

        private Color GetColor(int gameColorIndex)
        {
            int actualColorIndex = _rules.GetColor(gameColorIndex);
            return _colorsData.GetColor(actualColorIndex);
        }

        public Tile GetNextTile()
        {
            TileData data;

            switch (_type)
            {
                case TileType.Empty:
                    data = new EmptyTileData();
                    break;
                case TileType.Regular:
                    int colorIndex = _rules.GetColor(_firstGameColor);
                    data = new RegularTileData(_firstValue, colorIndex, 0);
                    break;
                case TileType.Mixed:
                    int topColorIndex = _rules.GetColor(_firstGameColor);
                    int bottomColorIndex = _rules.GetColor(_secondGameColor);
                    data = new MixedTileData(_firstValue, topColorIndex, _secondValue, bottomColorIndex, 0);
                    break;
                default:
                    Tile defaultTile = _factory.InstantiateTile(_type);
                    return defaultTile;
            }

            Tile tile = _factory.InstantiateTile(data);
            return tile;
        }
    }
}