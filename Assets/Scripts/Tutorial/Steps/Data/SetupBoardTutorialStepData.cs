using GameBoard;
using UnityEngine;

namespace Tutorial.Steps.Data
{
    public class SetupBoardTutorialStepData : TutorialStepData
    {
        [SerializeReference] public BoardData BoardConfiguration;
    }
}