using GameBoard;
using JetBrains.Annotations;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    [SerializeField] private Board _board;

    private void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            Destroy(gameObject);
            return;
        }
    }

    [UsedImplicitly]
    public void ResetBoard()
    {
        _board.Setup(_board.Size);
    }
}