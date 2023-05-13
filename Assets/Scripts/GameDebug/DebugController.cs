using GameBoard;
using JetBrains.Annotations;
using Tiles;
using UnityEngine;

namespace GameDebug
{
    public class DebugController : MonoBehaviour
    {
        [field: SerializeField] public bool DebugPlaceTiles { get; private set; }
        [field: SerializeField] public DebugTilePlacer TilePlacer { get; private set; }

        [SerializeField] private Board _board;

        public static bool IsDebug { get; private set; }

        private void Awake()
        {
            if (!Debug.isDebugBuild)
            {
                Destroy(gameObject);
                return;
            }

            IsDebug = true;
        }

        public void Setup()
        {
            TilePlacer.Setup();
        }

        public Tile GetTestTile()
        {
            return TilePlacer.GetNextTile();
        }

        [UsedImplicitly]
        public void ResetBoard()
        {
            _board.Setup(_board.Size);
        }
    }
}