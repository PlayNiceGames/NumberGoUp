using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Tiles;
using UnityEngine;

namespace GameTileQueue
{
    public class TileQueueAppearSequence : MonoBehaviour
    {
        private TileFactory _factory;
        private Transform _grid;
        private GameObject _firstTileBorder;
        private int _queueSize;

        public async UniTask Play()
        {
            SetBorderEnabled(false);

            List<VoidTile> voidTiles = InstantiateVoidTiles().ToList();
            await WaitForGridUpdate();
            
            

            SetBorderEnabled(true);
        }

        private IEnumerable<VoidTile> InstantiateVoidTiles()
        {
            for (int i = 0; i < _queueSize; i++)
            {
                VoidTile tile = _factory.InstantiateTile<VoidTile>();

                tile.SetParent(_grid);

                yield return tile;
            }
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