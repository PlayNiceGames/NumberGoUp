using System;
using UnityEngine;

namespace GameOver
{
    [Serializable]
    public class GameOverSettings
    {
        [field: SerializeField] public int MaxContinueCount { get; private set; }
        [field: SerializeField] public float ClearSmallTilesBiggestTileMultiplier { get; private set; }
    }
}