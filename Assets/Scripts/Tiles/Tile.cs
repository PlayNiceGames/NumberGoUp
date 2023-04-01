using System;
using UnityEngine;

namespace Tiles
{
    public abstract class Tile : MonoBehaviour, IDisposable
    {
        public Vector2Int BoardPosition;
        
        public abstract TileType Type { get; }
        
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent.transform, false);
        }
        
        public void ClearParent()
        {
            transform.SetParent(null, false);
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}