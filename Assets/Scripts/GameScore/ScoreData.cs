using System.Collections.Generic;
using System.Linq;
using Tiles;
using Tiles.Containers;
using UnityEngine;
using Utils;

namespace GameScore
{
    public class ScoreData : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<int, int> _scoreOverrideForRegularTileMerge;
        [SerializeField] private SerializedDictionary<int, int> _scoreOverrideForMixedTileMerge;
        [SerializeField] private int _regularTileScoreIncrement;
        [SerializeField] private int _mixedTileScoreIncrement;
        [Space] [SerializeField] private List<int> _analyticsReachedScoreOverrides;
        [SerializeField] private int _analyticsReachedScoreIncrement;

        public int GetScoreForMerge(int startingValue, int deltaValue, MergeContainer container)
        {
            int endValue = startingValue + deltaValue;
            int scoreSum = 0;


            for (int intermediateValue = startingValue + 1; intermediateValue <= endValue; intermediateValue++)
            {
                int scoreForValue = GetScoreForMerge(intermediateValue, container);

                scoreSum += scoreForValue;
            }

            return scoreSum;
        }

        private int GetScoreForMerge(int value, MergeContainer container)
        {
            int score = 0;

            if (container is RegularTileContainer)
            {
                score = GetScoreForRegularTileMerge(value);
            }
            else if (container is MixedTileContainer mixedTileContainer)
            {
                score = GetScoreForMixedTileMerge(value);

                if (mixedTileContainer.PartType == MixedTilePartType.Both)
                    score *= 2;
            }

            return score;
        }

        private int GetScoreForRegularTileMerge(int value)
        {
            int score = GetScoreForMerge(value, _regularTileScoreIncrement, _scoreOverrideForRegularTileMerge);
            return score;
        }

        private int GetScoreForMixedTileMerge(int value)
        {
            int score = GetScoreForMerge(value, _mixedTileScoreIncrement, _scoreOverrideForMixedTileMerge);
            return score;
        }

        private int GetScoreForMerge(int value, int scoreIncrement, Dictionary<int, int> overrides)
        {
            if (overrides.TryGetValue(value, out int overrideScore))
                return overrideScore;

            int lastOverrideValue = overrides.Keys.Last();
            int lastOverrideScore = overrides[lastOverrideValue];

            if (value > lastOverrideValue)
            {
                int valueDeltaFromLastOverride = value - lastOverrideValue;
                int score = valueDeltaFromLastOverride * scoreIncrement + lastOverrideScore;

                return score;
            }

            return 0;
        }

        public int? GetScoreReachedEventScoreRange(int score, out int rangeScore)
        {
            int overrideIndex = _analyticsReachedScoreOverrides.FindLastIndex(overrideScore => overrideScore <= score);

            if (overrideIndex == -1)
            {
                rangeScore = 0;
                return null;
            }

            int lastOverrideIndex = _analyticsReachedScoreOverrides.Count - 1;
            int overrideScore = _analyticsReachedScoreOverrides[overrideIndex];

            if (overrideIndex == lastOverrideIndex)
            {
                int lastOverrideScore = overrideScore;
                int nextRangeScore = overrideScore + _analyticsReachedScoreIncrement;

                if (nextRangeScore <= score)
                {
                    int incrementRange = (score - lastOverrideScore) / _analyticsReachedScoreIncrement;
                    int range = incrementRange + lastOverrideIndex;

                    rangeScore = lastOverrideScore + incrementRange * _analyticsReachedScoreIncrement;
                    return range;
                }
            }

            rangeScore = overrideScore;
            return overrideIndex;
        }
    }
}