using GameBoard;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    [SerializeField] private Board _board;

    private void Awake()
    {
        if (!EditorUserBuildSettings.development)
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