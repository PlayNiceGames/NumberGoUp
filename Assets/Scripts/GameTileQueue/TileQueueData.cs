using Tiles.Data;
using UnityEngine;

namespace GameTileQueue
{
    public class TileQueueData
    {
        [SerializeReference] public TileData[] Tiles;

        public TileQueueData(TileData[] tiles)
        {
            Tiles = tiles;
        }
    }
}