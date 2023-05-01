using System.Collections.Generic;
using GameBoard.Rules.Merge;
using GameBoard.Turns;
using GameScore;

namespace GameBoard.Rules
{
    public class BoardRules //TODO maybe move rules to RegularTile and MixedTile?
    {
        private Board _board;
        private ScoreSystem _scoreSystem;
        private List<BoardRule> _boardRules;

        public BoardRules(Board board, ScoreSystem scoreSystem)
        {
            _board = board;
            _scoreSystem = scoreSystem;

            InitializeRules();
        }

        private void InitializeRules()
        {
            _boardRules = new List<BoardRule>
            {
                new CenterMergeBoardRule(_board, _scoreSystem),
                new DoubleMergeBoardRule(_board, _scoreSystem),
                new SimpleMergeBoardRule(_board, _scoreSystem)
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