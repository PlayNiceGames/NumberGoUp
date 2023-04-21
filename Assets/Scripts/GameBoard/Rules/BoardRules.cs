using System.Collections.Generic;
using GameBoard.Turns;

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
                new CenterMergeBoardRule(_board),
                new MergeBoardRule(_board)
            };
        }

        public BoardTurn GetFirstAvailableTurn()
        {
            foreach (BoardRule rule in _boardRules)
            {
                BoardTurn turn = rule.GetTurn();

                if (turn != null)
                    return turn;
            }

            return null;
        }
    }
}