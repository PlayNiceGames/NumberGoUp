using Cysharp.Threading.Tasks;
using GameAudio;
using Tiles.Data;
using UnityEngine;

namespace Tiles
{
    public class EraserTile : Tile
    {
        public override TileType Type => TileType.Eraser;

        public override UniTask Appear(Vector2 position)
        {
            GameSounds.PlayEraser();

            return base.Appear(position);
        }

        public override TileData GetData()
        {
            return new EraserTileData();
        }

        public override bool Equals(Tile other)
        {
            return other is EraserTile;
        }
    }
}