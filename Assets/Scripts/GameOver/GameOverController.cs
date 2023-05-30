using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameBoard;
using GameScore;
using Tiles;
using Tiles.Data;
using UnityEngine;
using Utils;

namespace GameOver
{
    public class GameOverController
    {
        private GameOverUI _ui;
        private Board _board;
        private ScoreSystem _scoreSystem;
        private GameOverSettings _settings;

        private int _continueCount;

        public GameOverController(GameOverUI ui, Board board, ScoreSystem scoreSystem, GameOverSettings settings)
        {
            _ui = ui;
            _board = board;
            _scoreSystem = scoreSystem;
            _settings = settings;
        }

        public void Setup()
        {
            _ui.Setup();
        }

        public async UniTask<bool> TryProcessGameOver()
        {
            if (IsGameOver())
            {
                ValueTileData biggestTile = GetBiggestTile(out int biggestTileValue);

                int currentScore = _scoreSystem.Score;
                int highScore = _scoreSystem.HighScore;
                bool canContinueGame = _continueCount < _settings.MaxContinueCount;

                GameOverAction gameOverAction = await _ui.ShowWithResult(currentScore, highScore, biggestTile, canContinueGame);

                switch (gameOverAction)
                {
                    case GameOverAction.Exit:
                        return true;
                    case GameOverAction.Continue:
                        ClearSmallTiles(biggestTileValue);

                        _continueCount++;
                        return false;
                }
            }

            return false;
        }

        private ValueTileData GetBiggestTile(out int biggestTileValue)
        {
            List<ValueTile> tiles = _board.GetAllTiles<ValueTile>().ToList();

            if (tiles.Count == 0)
                throw new Exception("No value tiles on board. Can't end the game");

            ValueTile biggestTile = tiles.MaxBy(GetMaxValue);

            biggestTileValue = GetMaxValue(biggestTile);
            return (ValueTileData) biggestTile.GetData();
        }

        private void ClearSmallTiles(int biggestTileValue)
        {
            int tileValueThreshold = (int) (biggestTileValue * _settings.ClearSmallTilesBiggestTileMultiplier);

            IEnumerable<ValueTile> tiles = _board.GetAllTiles<ValueTile>();
            IEnumerable<ValueTile> smallTiles = tiles.Where(tile => GetMaxValue(tile) <= tileValueThreshold);

            foreach (ValueTile smallTile in smallTiles)
            {
                _board.ClearTile(smallTile.BoardPosition);
            }
        }

        private int GetMaxValue(ValueTile tile) //TODO maybe move somewhere?
        {
            if (tile is RegularTile regularTile)
                return regularTile.Value;

            if (tile is MixedTile mixedTile)
                return Mathf.Max(mixedTile.Top.Value, mixedTile.Bottom.Value);

            throw new ArgumentException("Unexpected value tile");
        }

        private bool IsGameOver()
        {
            IEnumerable<EmptyTile> emptyTiles = _board.GetAllTiles<EmptyTile>();

            return !emptyTiles.Any();
        }
    }
}