using System.Linq;
using Tiles;
using UnityEngine;
using Utils;

namespace GameScore
{
    public class ScoreData : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<int, int> _scoreForRegularTileMergeValue;
        [SerializeField] private SerializedDictionary<int, int> _scoreForMixedTileMergeValue;

        public int GetScoreForMerge(int startingValue, int deltaValue, TileType tileType)
        {
            int endValue = startingValue + deltaValue;
            int scoreSum = 0;

            for (int intermediateValue = startingValue + 1; intermediateValue <= endValue; intermediateValue++)
            {
                int scoreForValue = GetScoreForMerge(intermediateValue, tileType);

                scoreSum += scoreForValue;
            }

            return scoreSum;
        }

        private int GetScoreForMerge(int value, TileType type)
        {
            switch (type)
            {
                case TileType.Regular:
                    return GetScoreForRegularTileMerge(value);
                case TileType.Mixed:
                    return GetScoreForMixedTileMerge(value);
            }

            return 0;
        }

        private int GetScoreForRegularTileMerge(int value)
        {
            if (_scoreForMixedTileMergeValue.TryGetValue(value, out int score))
                return score;

            return _scoreForMixedTileMergeValue.Values.Last();
        }

        private int GetScoreForMixedTileMerge(int value)
        {
            if (_scoreForMixedTileMergeValue.TryGetValue(value, out int score))
                return score;

            return _scoreForMixedTileMergeValue.Values.Last();
        }
    }
}