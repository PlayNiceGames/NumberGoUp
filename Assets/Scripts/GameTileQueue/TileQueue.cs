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

        public Tile PopNextTile()
        {
            Tile firstTile = RemoveFirstTile();

            Tile nextTile = _tiles.Peek();
            nextTile.FadeAnimation.Fade(false);

            Tile newTile = AddNextTile();
            PlayAdvanceTileAnimation(newTile).Forget();

            return firstTile;
        }

        public Tile PeekNextTile()
        {
            return _tiles.TryPeek(out Tile result) ? result : null;
        }

        public async UniTask AddInitialTilesWithAnimation()
        {
            TileData[] initialTilesData = _generator.GetNextTilesData(QueueSize);
            List<Tile> initialTiles = await _appearSequence.Play(initialTilesData);

            _tiles = new Queue<Tile>(initialTiles);
        }

        private void AddInitialTiles()
        {
            ClearTiles();

            for (int i = 0; i < QueueSize; i++)
                AddNextTile();
        }

        private void ClearTiles()
        {
            foreach (Tile tile in _tiles)
                tile.ClearParent();

            _tiles.Clear();
        }

        private Tile AddNextTile()
        {
            Tile tile = InstantiateNextTile();
            tile.SetParent(_grid);

            _tiles.Enqueue(tile);

            return tile;
        }

        private async UniTask PlayAdvanceTileAnimation(Tile nextTile)
        {
            _advanceTileAnimationPlaying = new UniTaskCompletionSource();

            await nextTile.AppearAnimation.Play();

            _advanceTileAnimationPlaying.TrySetResult();
        }

        private Tile InstantiateNextTile()
        {
            TileData tileData = _generator.GetNextTileData();

            Tile tile = _factory.InstantiateTile(tileData);
            tile.FadeAnimation.Fade(true);
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

            return new TileQueueData(tilesData);
        }

        public void SetData(TileQueueData data)
        {
            foreach (TileData tileData in data.Tiles)
            {
                //Tile tile = 
            }
        }
    }
}