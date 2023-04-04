using System.Collections.Generic;
using GameRules;
using Tiles;
using UnityEngine;

namespace GameTileQueue
{
    public class TileQueueSet
    {
        private TileQueueGeneratorSettings _settings;
        private Rules _rules;

        private bool _guaranteedBigTile;
        private bool _guaranteedMixedTile;
        private List<ColorIndex> _guaranteedColorIndexes;

        private TileData[] _tiles;

        public TileQueueSet(TileQueueGeneratorSettings settings, Rules rules, bool guaranteedBigTile, bool guaranteedMixedTile, List<ColorIndex> guaranteedColorIndexes)
        {
            _settings = settings;
            _rules = rules;
            _guaranteedBigTile = guaranteedBigTile;
            _guaranteedMixedTile = guaranteedMixedTile;
            _guaranteedColorIndexes = guaranteedColorIndexes;

            _tiles = new TileData[_settings.TileQueueSize];

            Generate();
        }

        private void Generate()
        {
            TrySetGuaranteedColors();
            TrySetGuaranteedBigTile();
            TrySetGuaranteedMixedTile();
            TryFixRepeatingTiles();
            TryGenerateRemainingTiles();

            RecordTileAppearanceInQueue();
        }

        private void TrySetGuaranteedColors()
        {
            foreach (ColorIndex colorIndex in _guaranteedColorIndexes)
            {
                RegularTileData guaranteedTile = new RegularTileData(_settings.GuaranteedColorTileValue, colorIndex.Color);

                TrySetTile(colorIndex.Index, guaranteedTile);
            }
        }

        private void TrySetGuaranteedBigTile()
        {
            float randomValue = Random.value;

            if (_guaranteedBigTile || randomValue <= _settings.BigTileQueueGenerationChance)
            {
                int randomColor = _rules.GetRandomTileColor();
                RegularTileData regularTile = new RegularTileData(_settings.BigTileValue, randomColor);

                TrySetTileInRandomPlace(regularTile);
            }
        }

        private void TrySetGuaranteedMixedTile()
        {
            float randomValue = Random.value;

            if (_guaranteedMixedTile || randomValue <= _settings.MixedTileQueueGenerationChance)
            {
                (int topColor, int bottomColor) = _rules.GetRandomMixedTileColors();
                MixedTileData mixedTile = new MixedTileData(_settings.MixedTileValue, topColor, _settings.MixedTileValue, bottomColor);

                TrySetTileInRandomPlace(mixedTile);
            }
        }

        private void TryFixRepeatingTiles()
        {
            //throw new NotImplementedException();
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

            int randomIndex = freeTileIndexes[Random.Range(0, freeTileIndexes.Count)];

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
    }
}