using Tiles;

namespace TileQueue
{
    public class TileQueueGenerator
    {
        private TileFactory _factory;
        
        public TileQueueGenerator(TileFactory factory)
        {
            _factory = factory;
        }

        public Tile GetNextTile()
        {
            RegularTile tile = _factory.InstantiateTile<RegularTile>();
            tile.SetNumber(1);
            return tile;
        }
    }
}