using System.Collections.Generic;
using GameRules;
using Tiles;
using UnityEngine;
using Utils;

namespace GameTileQueue
{
    public class TileQueueSet
    {
        private TileQueueSet _prevSet;

        private TileQueueGeneratorSettings _settings;
        private Rules _rules;

        private bool _bigTileGenerated;
        private bool _mixedTileGenerated;
        private Dictionary<int, int> _colorNotAppearingCount;

        private TileData[] _tiles;

        public TileQueueSet(TileQueueSet prevSet, TileQueueGeneratorSettings settings, Rules rules)
        {
            _prevSet = prevSet;
            _settings = settings;
            _rules = rules;

            _tiles = new TileData[_settings.TileQueueSize];
        }

        public TileData[] Generate()
        {
            TrySetGuaranteedColors();
            TrySetGuaranteedBigTile();
            TrySetGuaranteedMixedTile();
            TryGenerateRemainingTiles();
            TryFixRepeatingTiles();

            RecordTileColorIndexes();

            Debug.Log($"Generated tile set");
            if (_bigTileGenerated)
                Debug.Log($"Generated big tile");
            if (_mixedTileGenerated)
                Debug.Log($"Generated mixed tile");

            foreach (KeyValuePair<int, int> i in _colorNotAppearingCount)
            {
                Debug.Log($"Color {i.Key} not appearing: {i.Value}");
            }

            return _tiles;
        }

        private void TrySetGuaranteedColors()
        {
            /*foreach (KeyValuePair<int, int> colorIndex in _colorNotAppearingCount)
            {
                RegularTileData guaranteedTile = new RegularTileData(_settings.GuaranteedColorTileValue, colorIndex.Key);

                TrySetTile(colorIndex.Value, guaranteedTile);
            }*/
        }

        private void TrySetGuaranteedBigTile()
        {
            float randomValue = Random.value;

            bool isBigTileGuaranteed = _prevSet != null && !_prevSet._bigTileGenerated;
            if (isBigTileGuaranteed || randomValue <= _settings.BigTileQueueGenerationChance)
            {
                int randomColor = _rules.GetRandomTileColor();
                RegularTileData regularTile = new RegularTileData(_settings.BigTileValue, randomColor);

                TrySetTileInRandomPlace(regularTile);

                _bigTileGenerated = true;
            }
        }

        private void TrySetGuaranteedMixedTile()
        {
            if (!_rules.CurrentRules.IncludeMixedTiles)
                return;

            float randomValue = Random.value;

            bool isMixedTileGuaranteed = _prevSet != null && !_prevSet._mixedTileGenerated;
            if (isMixedTileGuaranteed || randomValue <= _settings.MixedTileQueueGenerationChance)
            {
                (int topColor, int bottomColor) = _rules.GetRandomMixedTileColors();
                MixedTileData mixedTile = new MixedTileData(_settings.MixedTileValue, topColor, _settings.MixedTileValue, bottomColor);

                TrySetTileInRandomPlace(mixedTile);

                _mixedTileGenerated = true;
            }
        }

        private void TryFixRepeatingTiles()
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                if (_tiles[i] is not RegularTileData regularTile)
                    continue;

                int repeatCount = 0;

                for (int j = i + 1; j < _tiles.Length; j++)
                {
                    if (regularTile.Equals(_tiles[j]))
                        repeatCount++;
                }

                if (repeatCount > _settings.MaxRepeatingTileCount)
                {
                    int repeatingFixTileIndex = i + _settings.MaxRepeatingTileCount;
                    if (repeatingFixTileIndex >= _settings.TileQueueSize)
                    {
                        int randomColor = _rules.GetRandomTileColor();
                        _tiles[repeatingFixTileIndex] = new RegularTileData(_settings.RepeatingFixTileValue, randomColor);
                    }

                    Debug.Log($"Fixed repeating tile at: {repeatingFixTileIndex}");

                    return;
                }
            }
        }

        private void TryGenerateRemainingTiles()
        {
            for (int i = 0; i < _settings.TileQueueSize; i++)
            {
                if (IsLocked(i))
                    continue;

                int randomColor = _rules.GetRandomTileColor();
                RegularTileData regularTile = new RegularTileData(_settings.RemainingTileValue, randomColor);

                TrySetTile(i, regularTile);
            }
        }

        private bool TrySetTileInRandomPlace(TileData tile)
        {
            List<int> freeTileIndexes = new List<int>();

            for (int i = 0; i < _settings.TileQueueSize; i++)
            {
                if (!IsLocked(i))
                    freeTileIndexes.Add(i);
            }

            if (freeTileIndexes.Count == 0)
                return false;

            int randomIndex = freeTileIndexes.RandomIndex();
            _tiles[randomIndex] = tile;

            return true;
        }

        private bool TrySetTile(int index, TileData tile)
        {
            if (IsLocked(index))
                return false;

            _tiles[index] = tile;

            return true;
        }

        private bool IsLocked(int index)
        {
            return _tiles[index] != null;
        }

        private void RecordTileColorIndexes()
        {
            _colorNotAppearingCount = new Dictionary<int, int>();
            List<int> availableColors = _rules.GetAvailableColors();

            foreach (int color in availableColors)
            {
                if (TryGetColorLastAppearedIndex(color, out int index))
                {
                    _colorNotAppearingCount[color] = _settings.TileQueueSize - index - 1;
                }
                else
                {
                    if (_prevSet != null && _prevSet._colorNotAppearingCount.TryGetValue(color, out int prevColorNotAppearingCount))
                    {
                        _colorNotAppearingCount[color] = prevColorNotAppearingCount + _settings.TileQueueSize;
                    }
                    else
                    {
                        _colorNotAppearingCount[color] = 0;
                    }
                }
            }
        }

        private bool TryGetColorLastAppearedIndex(int color, out int lastAppearedIndex)
        {
            bool isColorFound = false;
            lastAppearedIndex = 0;

            for (int i = 0; i < _tiles.Length; i++)
            {
                TileData tile = _tiles[i];

                if (tile.ContainsColor(color))
                {
                    lastAppearedIndex = i;
                    isColorFound = true;
                }
            }

            return isColorFound;
        }
    }
}