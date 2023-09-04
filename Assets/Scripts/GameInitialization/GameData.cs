using GameBoard;
using GameTileQueue;

namespace GameInitialization
{
    public class GameData
    {
        public readonly BoardData BoardData;
        public readonly TileQueueData TileQueueData;
        public readonly int Score;

        public GameData(BoardData boardData, TileQueueData queueData, int score)
        {
            BoardData = boardData;
            TileQueueData = queueData;
            Score = score;
        }
    }
}