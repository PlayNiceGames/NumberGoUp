using System;
using UnityEngine;

namespace Tiles
{
    public abstract class Tile : MonoBehaviour, IDisposable
    {
        public Vector2Int BoardPosition;
        
        public abstract TileType Type { get; }

        protected Tile()
        {
        }

        protected Tile(Vector2Int boardPosition)
        {
            BoardPosition = boardPosition;
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}