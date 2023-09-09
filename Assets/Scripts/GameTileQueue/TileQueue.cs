using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameTileQueue.Generators;
using Serialization;
using Tiles;
using Tiles.Data;
using UnityEngine;

namespace GameTileQueue
{
    public class TileQueue : MonoBehaviour, IDataSerializable<TileQueueData>
    {
        private const int QueueSize = 4; //TODO temp, move

        [SerializeField] private Transform _grid;
        [SerializeField] private TileQueueAppearSequence _appearSequence;

        [SerializeField] private TileFactory _factory;

        private TileQueueGenerator _generator;
        private Queue<Tile> _tiles;

        private UniTaskCompletionSource _advanceTileAnimationPlaying;

        public void Setup(TileQueueGenerator generator)
        {
            _generator = generator;
            _tiles = new Queue<Tile>();
        }

        public UniTask SetupInitialTilesWithAnimation()
        {
            TileData[] initialTilesData = _generator.GetNextTilesData(QueueSize);
            return SetupTilesWithAnimation(initialTilesData);
        }

        public UniTask SetDataWithAnimation(TileQueueData data)
        {
            return SetupTilesWithAnimation(data.Tiles);
        }

        private async UniTask SetupTilesWithAnimation(TileData[] tileData)
        {
            ClearTiles();

            List<Tile> tiles = await _appearSequence.Play(tileData);
            _tiles = new Queue<Tile>(tiles);
        }

        public Tile PopNextTile()
        {
            Tile firstTile = RemoveFirstTile();

            Tile nextTile = _tiles.Peek();
            nextTile.FadeAnimation.Fade(false);

            Tile newTile = AddNextTile();
            newTile.FadeAnimation.Fade(true);
            PlayAdvanceTileAnimation(newTile).Forget();

            return firstTile;
        }

        private async UniTask PlayAdvanceTileAnimation(Tile nextTile)
        {
            _advanceTileAnimationPlaying = new UniTaskCompletionSource();

            await nextTile.AppearAnimation.Play();

            _advanceTileAnimationPlaying.TrySetResult();
        }

        public Tile PeekNextTile()
        {
            return _tiles.TryPeek(out Tile result) ? result : null;
        }

        private void ClearTiles()
        {
            if (_tiles == null)
                return;

            foreach (Tile tile in _tiles)
                tile.ClearParent();

            _tiles.Clear();
        }

        private Tile AddNextTile()
        {
            TileData tileData = _generator.GetNextTileData();
            Tile tile = AddTile(tileData);

            return tile;
        }

        private Tile AddTile(TileData tileData)
        {
            Tile tile = _factory.InstantiateTile(tileData);
            tile.SetParent(_grid);

            _tiles.Enqueue(tile);

            return tile;
        }

        private Tile RemoveFirstTile()
        {
            bool result = _tiles.TryDequeue(out Tile tile);

            if (!result)
                return null;

            tile.ClearParent();

            return tile;
        }

        public UniTask WaitUntilTileAdvances()
        {
            return _advanceTileAnimationPlaying?.Task ?? UniTask.CompletedTask;
        }

        public TileQueueData GetData()
        {
            TileData[] tilesData = _tiles.Select(tile => tile.GetData()).ToArray();
            TileQueueGeneratorData generatorData = _generator.GetData();

            return new TileQueueData(tilesData, generatorData);
        }

        public void SetData(TileQueueData data)
        {
            ClearTiles();

            _generator.SetData(data.GeneratorData);

            for (int i = 0; i < data.Tiles.Length; i++)
            {
                TileData tileData = data.Tiles[i];
                Tile tile = AddTile(tileData);

                tile.FadeAnimation.Fade(i == 0);
            }
        }
    }
}