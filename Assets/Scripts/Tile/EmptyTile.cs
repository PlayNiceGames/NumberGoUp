using UnityEngine;

namespace Tile
{
    public class EmptyTile : TileBase
    {
        public override TileType Type => TileType.Empty;
        
        public EmptyTile(Vector2Int boardPosition) : base(boardPosition)
        {
        }
    }
}