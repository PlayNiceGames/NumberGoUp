using System.Collections.Generic;
using GameBoard.Actions;

namespace GameBoard.Rules
{
    public class BoardRules //TODO maybe move rules to RegularTile and MixedTile?
    {
        private Board _board;
        private List<BoardRule> _boardRules;

        public BoardRules(Board board)
        {
            _board = board;

            InitializeRules();
        }

        private void InitializeRules()
        {
            _boardRules = new List<BoardRule>
            {
                new MergeBoardRule(_board)
            };
        }

        public BoardAction GetFirstAvailableTurn()
        {
            foreach (BoardRule rule in _boardRules)
            {
                BoardAction action = rule.GetAction();

                if (action != null)
                    return action;
            }

            return null;
        }
    }
}