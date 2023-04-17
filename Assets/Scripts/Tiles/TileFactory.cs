using System;
using Tiles.Data;
using UnityEngine;

namespace Tiles
{
    public class TileFactory : MonoBehaviour
    {
        [SerializeField] private EmptyTile _emptyTilePrefab;
        [SerializeField] private RegularTile _regularTilePrefab;
        [SerializeField] private MixedTile _mixedTilePrefab;

        public Tile InstantiateTile(TileData data)
        {
            if (data is EmptyTileData emptyTileData)
                return InstantiateTile<EmptyTile>();

            if (data is RegularTileData regularTileData)
            {
                RegularTile tile = InstantiateTile<RegularTile>();
                tile.Setup(regularTileData);

                return tile;
            }

            if (data is MixedTileData mixedTileData)
            {
                MixedTile tile = InstantiateTile<MixedTile>();
                tile.Setup(mixedTileData);

                return tile;
            }

            throw new ArgumentOutOfRangeException(nameof(data), "Wrong tile data");
        }

        public Tile InstantiateTile(TileType type)
        {
            Tile newTile = Instantiate(GetPrefab(type));
            return newTile;
        }

        public T InstantiateTile<T>() where T : Tile
        {
            if (_emptyTilePrefab is T emptyTile)
                return Instantiate(emptyTile);
            if (_regularTilePrefab is T regularTile)
                return Instantiate(regularTile);
            if (_mixedTilePrefab is T mixedTile)
                return Instantiate(mixedTile);

            throw new ArgumentOutOfRangeException(nameof(T), "Wrong tile type");
        }

        private Tile GetPrefab(TileType type)
        {
            switch (type)
            {
                case TileType.Empty:
                    return _emptyTilePrefab;
                case TileType.Regular:
                    return _regularTilePrefab;
                case TileType.Mixed:
                    return _mixedTilePrefab;
            }

            throw new ArgumentOutOfRangeException(nameof(type), type, "Wrong tile type");
        }
    }
}