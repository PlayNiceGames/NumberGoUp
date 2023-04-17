using GameBoard;
using JetBrains.Annotations;
using Tiles;
using UnityEngine;

namespace GameDebug
{
    public class DebugController : MonoBehaviour
    {
        public static bool IsDebug { get; private set; }
        [field: SerializeField] public bool DebugPlaceTiles { get; private set; }

        [SerializeField] private Board _board;
        [SerializeField] private DebugTilePlacer _tilePlacer;

        private void Awake()
        {
            if (!Debug.isDebugBuild)
            {
                Destroy(gameObject);
                return;
            }

            IsDebug = true;
        }

        public Tile GetTestTile()
        {
            return _tilePlacer.GetNextTile();
        }

        [UsedImplicitly]
        public void ResetBoard()
        {
            _board.Setup(_board.Size);
        }
    }
}