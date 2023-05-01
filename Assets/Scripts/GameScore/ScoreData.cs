using System.Linq;
using UnityEngine;
using Utils;

namespace GameScore
{
    public class ScoreData : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<int, int> _scoreForMergeValue;

        public int GetScoreForMerge(int mergeValue)
        {
            if (_scoreForMergeValue.TryGetValue(mergeValue, out int score))
                return score;

            return _scoreForMergeValue.Values.Last();
        }
    }
}