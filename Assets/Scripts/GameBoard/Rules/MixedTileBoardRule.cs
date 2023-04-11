using Tiles;

namespace GameBoard.Rules
{
    public abstract class MixedTileBoardRule : BoardRule
    {
        protected MixedTile _tile;

        public MixedTileBoardRule(MixedTile tile)
        {
            _tile = tile;
        }
    }
}