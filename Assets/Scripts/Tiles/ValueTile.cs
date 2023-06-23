using Cysharp.Threading.Tasks;
using Tiles.Animations;
using UnityEngine;

namespace Tiles
{
    public abstract class ValueTile : Tile
    {
        [SerializeField] protected TileMergeAnimation _mergeAnimation;

        public int Age;

        public abstract bool HasColor(int color);

        public virtual UniTask PlayMergeAnimation(Vector2 position)
        {
            return _mergeAnimation.Play(position);
        }
    }
}