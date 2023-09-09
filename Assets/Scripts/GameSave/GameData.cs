using System;
using GameBoard;
using GameLoop.Rules;
using GameTileQueue;

namespace GameSave
{
    [Serializable]
    public class GameData
    {
        public BoardData BoardData;
        public TileQueueData TileQueueData;
        public GameRulesData GameRulesData;
        public int Score;
        public int GameOverContinueCount;

        public GameData(BoardData boardData, TileQueueData tileQueueData, GameRulesData gameRulesData, int score, int gameOverContinueCount)
        {
            BoardData = boardData;
            TileQueueData = tileQueueData;
            GameRulesData = gameRulesData;
            Score = score;
            GameOverContinueCount = gameOverContinueCount;
        }
    }
}