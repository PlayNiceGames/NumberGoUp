using System;
using UnityEngine;

namespace Tile
{
    public abstract class TileBase : MonoBehaviour, IDisposable
    {
        public Vector2Int BoardPosition;

        protected TileBase()
        {
        }

        protected TileBase(Vector2Int boardPosition)
        {
            BoardPosition = boardPosition;
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}