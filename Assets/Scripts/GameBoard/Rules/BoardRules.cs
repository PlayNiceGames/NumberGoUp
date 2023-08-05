using System.Collections.Generic;
using Analytics;
using Cysharp.Threading.Tasks;
using GameBoard.Rules.Merge;
using GameBoard.Turns;
using GameScore;
using ServiceLocator;
using Tiles;
using UnityEngine;

namespace GameBoard.Rules
{
    public class BoardRules
    {
        private List<BoardRule> _boardRules;

        private Board _board;
        private ScoreSystem _scoreSystem;
        private TileFactory _factory;
        private AnalyticsService _analytics;

        public BoardRules(Board board, ScoreSystem scoreSystem, TileFactory factory)
        {
            _board = board;
            _scoreSystem = scoreSystem;
            _factory = factory;

            _analytics = GlobalServices.Get<AnalyticsService>();

            InitializeRules();
        }

        private void InitializeRules()
        {
            _boardRules = new List<BoardRule>
            {
                new EraserBoardRule(_board),
                new CenterMergeBoardRule(_board, _scoreSystem, _analytics),
                new DoubleMergeBoardRule(_board, _factory, _scoreSystem, _analytics),
                new SimpleMergeBoardRule(_board, _scoreSystem)
            };
        }

        public async UniTask ProcessRules()
        {
            while (true)
            {
                BoardTurn turn = GetFirstAvailableTurn();

                if (turn == null)
                    return;

                await turn.Run();
                await UniTask.Yield();

                Debug.Log($"RAN turn {turn.GetType()}");
            }
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