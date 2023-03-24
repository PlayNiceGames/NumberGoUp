using UnityEngine;

namespace Tile
{
    public abstract class TileBase : MonoBehaviour
    {
        public Vector2Int _boardPosition;

        protected TileBase()
        {
            
        }
        
        protected TileBase(Vector2Int boardPosition)
        {
            _boardPosition = boardPosition;
        }
    }
}