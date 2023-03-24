using Tile.UI;
using UnityEngine;

namespace Tile
{
    public class TileFactory : MonoBehaviour
    {
        [SerializeField] private EmptyTileUI _emptyTilePrefab;
        
        public EmptyTile InstantiateEmptyTile()
        {
            EmptyTileUI tileObject = Instantiate(_emptyTilePrefab);
            EmptyTile tile = new EmptyTile(tileObject);
            return tile;
        }
    }
}