using Tiles.Data;

namespace GameTileQueue
{
    public class TutorialTileQueueGenerator : TileQueueGenerator
    {
        private readonly TileData _defaultTile;

        public TutorialTileQueueGenerator(TileData defaultTile)
        {
            _defaultTile = defaultTile;
        }

        public override TileData GetNextTileData()
        {
            return _defaultTile;
        }
    }
}