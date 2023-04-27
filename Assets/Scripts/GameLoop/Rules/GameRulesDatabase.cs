using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLoop.Rules
{
    public class GameRulesDatabase : ScriptableObject
    {
        [field: SerializeField] private List<GameRulesData> _rulesSet;

        public GameRulesData GetInitialRules()
        {
            return _rulesSet[0];
        }

        public GameRulesData GetRules(int score)
        {
            return _rulesSet.LastOrDefault(rule => rule.RuleApplyStartingScore <= score);
        }
    }
}