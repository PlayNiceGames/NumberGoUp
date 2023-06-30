using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameTileQueue.Generators;
using Tiles;
using Tiles.Data;
using UnityEngine;

namespace GameTileQueue
{
    public class TileQueue : MonoBehaviour
    {
        private const int QueueSize = 4; //TODO temp, move

        [SerializeField] private Transform _grid;
        [SerializeField] private GameObject _firstTileBorder;
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

            Tile nextTile = AddNextTile();

            PlayAdvanceTileAnimation(nextTile).Forget();

            return firstTile;
        }

        public Tile PeekNextTile()
        {
            return _tiles.TryPeek(out Tile result) ? result : null;
        }

        public async UniTask AddInitialTilesWithAnimation()
        {
            AddInitialTiles();

            await PlayTileAppearAnimations();

            SetBorderEnabled(true);
        }

        private async UniTask PlayTileAppearAnimations()
        {
            for (int i = QueueSize - 1; i >= 0; i--)
            {
                Tile tile = _tiles.ElementAt(i);

                await tile.AppearAnimation.Play();
            }
        }

        public void AddInitialTiles()
        {
            ClearTiles();

            for (int i = 0; i < QueueSize; i++)
            {
                AddNextTile();
            }
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

        public UniTask WaitUntilFree()
        {
            return _advanceTileAnimationPlaying?.Task ?? UniTask.CompletedTask;
        }
    }
}