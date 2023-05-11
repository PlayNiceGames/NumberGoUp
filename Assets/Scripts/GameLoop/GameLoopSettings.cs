using GameOver;
using UnityEngine;

namespace GameLoop
{
    public class GameLoopSettings : ScriptableObject
    {
        [field: SerializeField] public GameOverSettings GameOverSettings { get; private set; }
    }
}