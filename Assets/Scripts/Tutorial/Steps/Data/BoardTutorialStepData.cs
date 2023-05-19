using GameBoard;
using JetBrains.Annotations;
using UnityEngine;

namespace Tutorial.Data
{
    [UsedImplicitly]
    public struct BoardTutorialStepData : ITutorialStepData
    {
        [SerializeReference] public BoardData BoardConfiguration;
    }
}