﻿using System.Collections.Generic;
using GameLoop.Rules;
using Tiles.Data;
using UnityEngine;
using Utils;

namespace GameTileQueue
{
    public class TileQueueSet
    {
        private TileQueueSet _prevSet;

        private TileQueueGeneratorSettings _settings;
        private GameRules _rules;
        private int _currentScore;

        private bool _bigTileGenerated;
        private bool _mixedTileGenerated;
        private int? _prevGeneratedEraserTileScore;
        private Dictionary<int, int> _colorNotAppearingCount;

        private TileData[] _tiles;

        public TileQueueSet(TileQueueSet prevSet, TileQueueGeneratorSettings settings, GameRules rules, int currentScore)
        {
            _prevSet = prevSet;
            _settings = settings;
            _rules = rules;
            _currentScore = currentScore;

            _colorNotAppearingCount = new Dictionary<int, int>();
            _tiles = new TileData[_settings.TileQueueSize];
        }

        public TileData[] Generate()
        {
            TryGenerateEraserTile();
            TrySetGuaranteedColors();
            TrySetGuaranteedBigTile();
            TrySetGuaranteedMixedTile();
            TryGenerateRemainingTiles();
            TryFixRepeatingTiles();

            RecordTileColorIndexes();

            return _tiles;
        }

        private void TryGenerateEraserTile()
        {
            if (!ShouldGenerateEraserTile(_currentScore))
                return;

            if (_prevSet?._prevGeneratedEraserTileScore == null)
            {
                GenerateEraserTile();
                return;
            }

            int scoreRange = GetEraserTileScoreRange(_currentScore);
            int prevScoreRange = GetEraserTileScoreRange(_prevSet._prevGeneratedEraserTileScore.Value);

            if (scoreRange == prevScoreRange)
                _prevGeneratedEraserTileScore = _prevSet._prevGeneratedEraserTileScore;
            else
                GenerateEraserTile();
        }

        private void GenerateEraserTile()
        {
            EraserTileData eraserTileData = new EraserTileData();

            if (TrySetTileInRandomPlace(eraserTileData))
            {
                _prevGeneratedEraserTileScore = _currentScore;

                Debug.Log("Placed eraser tile");
            }
            else
            {
                Debug.LogWarning("Unable to place eraser tile");
            }
        }

        private bool ShouldGenerateEraserTile(int score)
        {
            return score >= _settings.EraserTileStartingScore;
        }

        private int GetEraserTileScoreRange(int score)
        {
            return score / _settings.EraserTileScoreIncrement;
        }

        private void TrySetGuaranteedColors()
        {
            if (_prevSet == null)
                return;

            foreach (KeyValuePair<int, int> colorCount in _prevSet._colorNotAppearingCount)
            {
                TryPlaceGuaranteedColorTile(colorCount);
            }
        }

        private void TryPlaceGuaranteedColorTile(KeyValuePair<int, int> colorCount)
        {
            for (int j = 0; j < _tiles.Length; j++)
            {
                int colorNotAppearingCountForTile = colorCount.Value + j;

                if (colorNotAppearingCountForTile >= _settings.GuaranteedColorNotAppearingMaxCount)
                {
                    RegularTileData guaranteedColorTile = new RegularTileData(_settings.GuaranteedColorTileValue, colorCount.Key);

                    if (TrySetTile(j, guaranteedColorTile))
                        Debug.Log($"Placed guaranteed color tile: {j}");
                    else
                        Debug.LogWarning($"Unable to place guaranteed color tile: {j}");

                    return;
                }
            }
        }

        private void TrySetGuaranteedBigTile()
        {
            if (!_rules.CurrentRules.IncludeBigTiles)
                return;

            float randomValue = Random.value;

            bool isBigTileGuaranteed = _prevSet != null && !_prevSet._bigTileGenerated;
            if (isBigTileGuaranteed || randomValue <= _settings.BigTileQueueGenerationChance)
            {
                int randomColor = _rules.GetRandomTileColor();
                RegularTileData regularTile = new RegularTileData(_settings.BigTileValue, randomColor);

                if (TrySetTileInRandomPlace(regularTile))
                {
                    Debug.Log("Placed big tile");
                    _bigTileGenerated = true;
                }
                else
                {
                    Debug.LogWarning($"Unable to place big tile");
                }
            }
        }

        private void TrySetGuaranteedMixedTile()
        {
            if (!_rules.CurrentRules.IncludeMixedTiles)
                return;

            float randomValue = Random.value;

            bool isMixedTileGuaranteed = _prevSet != null && !_prevSet._mixedTileGenerated;
            if (isMixedTileGuaranteed || randomValue <= _rules.CurrentRules.MixedTileRules.QueueGenerationChance)
            {
                (int topColor, int bottomColor) = _rules.GetRandomMixedTileColors();
                MixedTileData mixedTile = new MixedTileData(_settings.MixedTileValue, topColor, _settings.MixedTileValue, bottomColor);

                if (TrySetTileInRandomPlace(mixedTile))
                {
                    Debug.Log("Placed mixed tile");
                    _mixedTileGenerated = true;
                }
                else
                {
                    Debug.LogWarning($"Unable to place mixed tile");
                }
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
                    if (repeatingFixTileIndex < _settings.TileQueueSize)
                    {
                        if (_rules.CurrentRules.AvailableColorCount <= 1)
                        {
                            Debug.LogWarning($"Unable to fix repeating tile at: {repeatingFixTileIndex}");

                            return;
                        }

                        int randomColor = _rules.GetRandomTileColorExcept(regularTile.Color);
                        _tiles[repeatingFixTileIndex] = new RegularTileData(_settings.RepeatingFixTileValue, randomColor);

                        Debug.Log($"Fixed repeating tile at: {repeatingFixTileIndex}");
                    }
                    else
                    {
                        Debug.LogWarning($"Unable to fix repeating tile at: {repeatingFixTileIndex}");
                    }

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
            _colorNotAppearingCount.Clear();
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
                        _colorNotAppearingCount[color] = prevColorNotAppearingCount + _settings.TileQueueSize;
                    else
                        _colorNotAppearingCount[color] = _settings.GuaranteedColorNotAppearingMaxCount;
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

                if (tile is not ValueTileData valueTileData)
                    continue;

                if (valueTileData.HasColor(color))
                {
                    lastAppearedIndex = i;
                    isColorFound = true;
                }
            }

            return isColorFound;
        }
    }
}