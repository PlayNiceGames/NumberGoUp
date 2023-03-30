using UnityEngine;

namespace Tiles
{
    public class EmptyTile : Tile
    {
        public override TileType Type => TileType.Empty;
        
        public EmptyTile(Vector2Int boardPosition) : base(boardPosition)
        {
        }
    }
}