using System;
using UnityEngine;

namespace GameOver
{
    [Serializable]
    public class GameOverSettings
    {
        [field: SerializeField] public float MaxContinueCount { get; private set; }
        [field: SerializeField] public float ClearSmallTilesBiggestTileMultiplier { get; private set; }
    }
}