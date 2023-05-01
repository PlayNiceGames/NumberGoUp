using System.Linq;
using UnityEngine;
using Utils;

namespace GameScore
{
    public class ScoreSettings : ScriptableObject
    {
        [SerializeField] private UnitySerializedDictionary<int, int> _scoreForMergeValue;

        public int GetScoreForMerge(int mergeValue)
        {
            if (_scoreForMergeValue.TryGetValue(mergeValue, out int score))
                return score;

            return _scoreForMergeValue.Values.Last();
        }
    }
}