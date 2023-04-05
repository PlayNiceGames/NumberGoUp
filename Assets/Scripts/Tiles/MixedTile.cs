using UnityEngine;

namespace Tiles
{
    public class MixedTile : Tile
    {
        public override TileType Type => TileType.Mixed;

        [field: SerializeField] public MixedTileModel Top { get; private set; }
        [field: SerializeField] public MixedTileModel Bottom { get; private set; }
        
        public void Setup(MixedTileData mixedTileData)
        {
            Top.Setup(mixedTileData.TopValue, mixedTileData.TopColor);
            Bottom.Setup(mixedTileData.BottomValue, mixedTileData.BottomColor);
        }
    }
}