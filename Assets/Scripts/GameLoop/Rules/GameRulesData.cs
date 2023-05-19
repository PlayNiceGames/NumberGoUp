using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLoop.Rules
{
    public class GameRulesData : ScriptableObject
    {
        [SerializeField] private List<GameRulesSet> _rulesSets;

        public GameRulesSet GetInitialRules()
        {
            return _rulesSets[0];
        }

        public GameRulesSet GetRules(int score)
        {
            return _rulesSets.LastOrDefault(rule => rule.RuleApplyStartingScore <= score);
        }
    }
}