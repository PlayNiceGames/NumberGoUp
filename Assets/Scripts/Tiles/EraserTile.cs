using Cysharp.Threading.Tasks;
using GameAudio;
using ServiceLocator;
using Tiles.Data;
using UnityEngine;

namespace Tiles
{
    public class EraserTile : Tile
    {
        public override TileType Type => TileType.Eraser;

        private GameAudio.Audio _audio;

        private void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
        }

        public override UniTask Appear(Vector2 position)
        {
            _audio.PlayEraser();

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