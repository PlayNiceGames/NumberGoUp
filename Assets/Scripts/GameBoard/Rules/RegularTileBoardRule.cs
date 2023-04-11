using Tiles;

namespace GameBoard.Rules
{
    public abstract class RegularTileBoardRule : BoardRule
    {
        protected RegularTile _tile;
        
        public RegularTileBoardRule(RegularTile tile)
        {
            _tile = tile;
        }
    }
}