using System;
using GameLoop.Rules;
using Sirenix.OdinInspector;
using Tiles;
using Tiles.Data;
using UnityEngine;

namespace GameDebug
{
    public class DebugTilePlacer : MonoBehaviour
    {
        [SerializeField] private int _value;
        [SerializeField] private int _gameColorIndex;
        [SerializeField] private TileType _type;

        [Space]
        [SerializeField] [ReadOnly] private Color _displayedColor;

        [Space]
        [SerializeField] private TileFactory _factory;

        [SerializeField] private GameRules _rules;
        [SerializeField] private TileColorsDatabase _colorsDatabase;

        private void Update()
        {
            if (!Application.isPlaying)
                return;

            int actualColorIndex = _rules.GetColor(_gameColorIndex);
            _displayedColor = _colorsDatabase.GetColor(actualColorIndex);
        }

        public Tile GetNextTile()
        {
            int actualColorIndex = _rules.GetColor(_gameColorIndex);

            TileData data;

            switch (_type)
            {
                case TileType.Empty:
                    data = new EmptyTileData();
                    break;
                case TileType.Regular:
                    data = new RegularTileData(_value, actualColorIndex);
                    break;
                case TileType.Mixed:
                    data = new MixedTileData(_value, actualColorIndex, _value, actualColorIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Tile tile = _factory.InstantiateTile(data);
            return tile;
        }
    }
}