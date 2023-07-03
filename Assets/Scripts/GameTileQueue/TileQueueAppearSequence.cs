using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Tiles;
using Tiles.Data;
using UnityEngine;

namespace GameTileQueue
{
    public class TileQueueAppearSequence : MonoBehaviour
    {
        [SerializeField] private TileFactory _factory;
        [SerializeField] private Transform _grid;
        [SerializeField] private GameObject _firstTileBorder;

        public async UniTask<List<Tile>> Play(TileData[] initialTilesData)
        {
            int tilesCount = initialTilesData.Length;

            SetBorderEnabled(false);

            List<VoidTile> voidTiles = InstantiateVoidTiles(tilesCount).ToList();
            await WaitForGridUpdate();

            List<Tile> initialTiles = await InstantiateInitialTiles(initialTilesData, voidTiles);

            SetBorderEnabled(true);

            return initialTiles;
        }

        private IEnumerable<VoidTile> InstantiateVoidTiles(int tilesCount)
        {
            for (int i = 0; i < tilesCount; i++)
            {
                VoidTile tile = _factory.InstantiateTile<VoidTile>();

                tile.SetParent(_grid);

                yield return tile;
            }
        }

        private async UniTask<List<Tile>> InstantiateInitialTiles(TileData[] initialTilesData, List<VoidTile> voidTiles)
        {
            List<Tile> tiles = new List<Tile>();
            List<UniTask> tileAppearTasks = new List<UniTask>();

            for (int i = initialTilesData.Length - 1; i >= 0; i--)
            {
                VoidTile voidTile = voidTiles[i];
                voidTile.Dispose();

                TileData tileData = initialTilesData[i];
                Tile tile = _factory.InstantiateTile(tileData);

                tile.SetParent(_grid);
                tile.transform.SetSiblingIndex(i + 1);

                tiles.Add(tile);

                UniTask appearTask = tile.AppearAnimation.Play();
                tileAppearTasks.Add(appearTask);

                await tile.AppearAnimation.WaitForTilesAppearDelay();
            }

            await UniTask.WhenAll(tileAppearTasks);

            tiles.Reverse();
            return tiles;
        }

        private async UniTask WaitForGridUpdate()
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        }

        private void SetBorderEnabled(bool isEnabled)
        {
            _firstTileBorder.SetActive(isEnabled);
        }
    }
}