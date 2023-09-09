using GameBoard;
using GameLoop.Rules;
using GameOver;
using GameScore;
using GameTileQueue;
using Serialization;
using UnityEngine;

namespace GameSave
{
    public class GameDataSerializer : MonoBehaviour, IDataSerializable<GameData>
    {
        [SerializeField] private Board _board;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private GameRules _gameRules;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private GameOverController _gameOver;

        public GameData GetData()
        {
            BoardData boardData = _board.GetData();
            TileQueueData tileQueueData = _tileQueue.GetData();
            GameRulesData gameRulesData = _gameRules.GetData();
            int score = _scoreSystem.GetData();
            int gameOverContinueCount = _gameOver.GetData();

            return new GameData(boardData, tileQueueData, gameRulesData, score, gameOverContinueCount);
        }

        public void SetData(GameData data)
        {
            BoardData boardData = data.BoardData;
            TileQueueData tileQueueData = data.TileQueueData;
            GameRulesData gameRulesData = data.GameRulesData;
            int score = data.Score;
            int gameOverContinueCount = data.GameOverContinueCount;

            _board.SetData(boardData);
            _tileQueue.SetData(tileQueueData);
            _gameRules.SetData(gameRulesData);
            _scoreSystem.SetData(score);
            _gameOver.SetData(gameOverContinueCount);
        }
    }
}