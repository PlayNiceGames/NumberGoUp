using GameBoard;
using UnityEngine;

namespace Tutorial.Data
{
    public struct SetupBoardTutorialStepData : ITutorialStepData
    {
        [SerializeReference] public BoardData BoardConfiguration;
    }
}