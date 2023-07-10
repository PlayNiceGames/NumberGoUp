using System;
using Cysharp.Threading.Tasks;
using Tiles.Animations;
using Tiles.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Tiles
{
    public abstract class Tile : MonoBehaviour, IDisposable, IEquatable<Tile>
    {
        public Vector2Int BoardPosition;

        public event Action<Tile> OnClick;

        [field: SerializeField] public TileAppearAnimation AppearAnimation { get; private set; }

        [SerializeField] protected LayoutElement _layout;

        public abstract TileType Type { get; }

        public virtual UniTask Appear(Vector2 position)
        {
            transform.position = position;

            return AppearAnimation.Play();
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent.transform, false);
        }

        public void ClearParent()
        {
            transform.SetParent(null, false);
        }

        public void SetIgnoreGrid(bool ignoreGrid)
        {
            if (_layout.ignoreLayout == ignoreGrid)
                return;

            _layout.ignoreLayout = ignoreGrid;
        }

        public void OnClicked()
        {
            OnClick?.Invoke(this);
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }

        public abstract TileData GetData();
        public abstract bool Equals(Tile other);
    }
}