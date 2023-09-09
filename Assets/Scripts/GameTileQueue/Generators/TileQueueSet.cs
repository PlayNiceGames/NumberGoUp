using System.Collections.Generic;
using GameLoop.Rules;
using GameScore;
using Serialization;
using Tiles.Data;
using UnityEngine;
using Utils;

namespace GameTileQueue.Generators
{
    public class TileQueueSet : IDataSerializable<TileQueueSetData>
    {
        private readonly TileQueueSet _prevSet;

        private readonly TileQueueGeneratorSettings _settings;
        private readonly GameRules _rules;
        private readonly ScoreSystem _scoreSystem;

        private bool _bigTileGenerated;
        private bool _mixedTileGenerated;
        private int? _prevGeneratedEraserTileScore;
        private Dictionary<int, int> _colorsNotAppearingCount;

        private TileData[] _tiles;

        public TileQueueSet(TileQueueSet prevSet, TileQueueGeneratorSettings settings, GameRules rules, ScoreSystem scoreSystem)
        {
            _prevSet = prevSet;
            _settings = settings;
            _rules = rules;
            _scoreSystem = scoreSystem;

            _colorsNotAppearingCount = new Dictionary<int, int>();
        }

        public TileData[] Generate()
        {
            _tiles = new TileData[_settings.TileQueueSize];

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
            int currentScore = _scoreSystem.Score;

            if (!ShouldGenerateEraserTile(currentScore))
                return;

            if (_prevSet?._prevGeneratedEraserTileScore == null)
            {
                GenerateEraserTile(currentScore);
                return;
            }

            int scoreRange = GetEraserTileScoreRange(currentScore);
            int prevScoreRange = GetEraserTileScoreRange(_prevSet._prevGeneratedEraserTileScore.Value);

            if (scoreRange == prevScoreRange)
                _prevGeneratedEraserTileScore = _prevSet._prevGeneratedEraserTileScore;
            else
                GenerateEraserTile(currentScore);
        }

        private void GenerateEraserTile(int score)
        {
            EraserTileData eraserTileData = new EraserTileData();

            if (TrySetTileInRandomPlace(eraserTileData))
            {
                _prevGeneratedEraserTileScore = score;

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

            foreach (KeyValuePair<int, int> colorCount in _prevSet._colorsNotAppearingCount)
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
                    Debug.LogWarning("Unable to place big tile");
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
                    Debug.LogWarning("Unable to place mixed tile");
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
            _colorsNotAppearingCount.Clear();
            List<int> availableColors = _rules.GetAvailableColors();

            foreach (int color in availableColors)
            {
                if (TryGetColorLastAppearedIndex(color, out int index))
                {
                    _colorsNotAppearingCount[color] = _settings.TileQueueSize - index - 1;
                }
                else
                {
                    if (_prevSet != null && _prevSet._colorsNotAppearingCount.TryGetValue(color, out int prevColorNotAppearingCount))
                        _colorsNotAppearingCount[color] = prevColorNotAppearingCount + _settings.TileQueueSize;
                    else
                        _colorsNotAppearingCount[color] = _settings.GuaranteedColorNotAppearingMaxCount;
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

        public TileQueueSetData GetData()
        {
            return new TileQueueSetData(_bigTileGenerated, _mixedTileGenerated, _prevGeneratedEraserTileScore, _colorsNotAppearingCount);
        }

        public void SetData(TileQueueSetData data)
        {
            _bigTileGenerated = data.BigTileGenerated;
            _mixedTileGenerated = data.MixedTileGenerated;
            _prevGeneratedEraserTileScore = data.PrevGeneratedEraserTileScore;
            _colorsNotAppearingCount = data.ColorsNotAppearingCount;
        }
    }
}