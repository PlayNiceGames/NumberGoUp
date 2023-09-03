using GameBoard;
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
        [SerializeField] private ScoreSystem _scoreSystem;

        public GameData GetData()
        {
            BoardData boardData = _board.GetData();
            TileQueueData tileQueueData = _tileQueue.GetData();
            int score = _scoreSystem.GetData();

            return new GameData(boardData, tileQueueData, score);
        }

        public void SetData(GameData data)
        {
            BoardData boardData = data.BoardData;
            TileQueueData tileQueueData = data.TileQueueData;
            int score = data.Score;

            _board.SetData(boardData);
            _tileQueue.SetData(tileQueueData);
            _scoreSystem.SetData(score);
        }
    }
}