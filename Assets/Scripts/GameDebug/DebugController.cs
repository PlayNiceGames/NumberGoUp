using GameBoard;
using JetBrains.Annotations;
using UnityEngine;

namespace GameDebug
{
    public class DebugController : MonoBehaviour
    {
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

        [UsedImplicitly]
        public void ResetBoard()
        {
            BoardData testData = BoardData.Square(_board.Size);
            _board.SetData(testData);
        }
    }
}