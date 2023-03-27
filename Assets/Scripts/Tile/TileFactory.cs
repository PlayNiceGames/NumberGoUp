using System;
using UnityEngine;

namespace Tile
{
    public class TileFactory : MonoBehaviour
    {
        [SerializeField] private EmptyTile _emptyTilePrefab;
        [SerializeField] private RegularTile _regularTilePrefab;

        public TileBase InstantiateTile(TileType type)
        {
            TileBase newTile = Instantiate(GetPrefab(type));
            return newTile;
        }

        public T InstantiateTile<T>() where T : TileBase
        {
            if (_emptyTilePrefab is T emptyTile)
                return Instantiate(emptyTile);
            if (_regularTilePrefab is T regularTile)
                return Instantiate(regularTile);

            throw new ArgumentOutOfRangeException(nameof(T), "Wrong tile type");
        }

        private TileBase GetPrefab(TileType type)
        {
            switch (type)
            {
                case TileType.Empty:
                    return _emptyTilePrefab;
                case TileType.Regular:
                    return _regularTilePrefab;
                case TileType.Mixed:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(type), type, "Wrong tile type");
        }
    }
}