using GameBoard;
using UnityEngine;

namespace Tutorial.Steps.Data
{
    public struct SetupBoardTutorialStepData : ITutorialStepData
    {
        [SerializeReference] public BoardData BoardConfiguration;
    }
}