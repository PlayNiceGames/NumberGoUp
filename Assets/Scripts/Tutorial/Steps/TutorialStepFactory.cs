using System;
using GameBoard;
using GameLoop;
using Tutorial.Data;
using UnityEngine;

namespace Tutorial
{
    public class TutorialStepFactory : MonoBehaviour //TODO remove after Zenject
    {
        [SerializeField] private Board _board;
        [SerializeField] private BoardGameLoop _boardLoop;
        [SerializeField] private TutorialDialogUI _dialogUI;

        public TutorialStep CreateStep(ITutorialStepData data)
        {
            switch (data)
            {
                case SetupBoardTutorialStepData setupBoardStepData:
                    return new SetupBoardTutorialStep(setupBoardStepData, _board);
                case BoardTurnTutorialStepData boardTurnStepData:
                    return new BoardTurnTutorialStep(boardTurnStepData, _boardLoop);
                case DialogTutorialStepData dialogStepData:
                    return new DialogTutorialStep(dialogStepData, _dialogUI);
            }

            throw new ArgumentException();
        }
    }
}