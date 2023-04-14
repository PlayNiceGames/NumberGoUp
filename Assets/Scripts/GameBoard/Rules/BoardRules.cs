using System.Collections.Generic;
using Tiles;
using UnityEngine;

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
                new TestBoardRule(_board)
            };
        }

        public BoardTurn GetFirstAvailableTurn()
        {
            foreach (BoardRule rule in _boardRules)
            {
                BoardTurn turn = GetFirstAvailableTurn(rule);

                if (turn != null)
                    return turn;
            }

            return null;
        }

        private BoardTurn GetFirstAvailableTurn(BoardRule rule)
        {
            Vector2Int position = Vector2Int.zero;
            for (position.x = 0; position.x < _board.Size; position.x++)
            {
                for (position.y = 0; position.y < _board.Size; position.y++)
                {
                    Tile tile = _board.GetTile(position);

                    BoardTurn turn = rule.GetAction(tile);

                    if (turn != null)
                        return turn;
                }
            }

            return null;
        }
    }
}