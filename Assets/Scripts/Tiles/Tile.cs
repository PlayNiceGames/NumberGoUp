using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tiles
{
    public abstract class Tile : MonoBehaviour, IDisposable
    {
        public Vector2Int BoardPosition;

        public event Action<Tile> OnClick;

        [SerializeField] protected LayoutElement _layout;

        public abstract TileType Type { get; }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent.transform, false);
            transform.SetAsLastSibling();
        }

        public void ClearParent()
        {
            transform.SetParent(null, false);
        }

        public void OnClicked()
        {
            OnClick?.Invoke(this);
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}