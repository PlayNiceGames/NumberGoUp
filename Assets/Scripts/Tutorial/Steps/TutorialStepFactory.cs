using System;
using GameBoard;
using GameLoop;
using Tiles;
using Tutorial.Dialog;
using Tutorial.Steps.Data;
using UnityEngine;

namespace Tutorial.Steps
{
    public class TutorialStepFactory : MonoBehaviour //TODO remove after Zenject
    {
        [SerializeField] private Board _board;
        [SerializeField] private BoardGameLoop _boardLoop;
        [SerializeField] private TileFactory _tileFactory;
        [SerializeField] private TutorialDialogController _dialogController;

        public TutorialStep CreateStep(TutorialStepData data)
        {
            switch (data)
            {
                case DelayTutorialStepData delayStepData:
                    return new DelayTutorialStep(delayStepData);
                case SetupBoardTutorialStepData setupBoardStepData:
                    return new SetupBoardTutorialStep(setupBoardStepData, _tileFactory, _board);
                case BoardTurnTutorialStepData boardTurnStepData:
                    return new BoardTurnTutorialStep(boardTurnStepData, _boardLoop);
                case DialogTutorialStepData dialogStepData:
                    return new DialogTutorialStep(dialogStepData, _dialogController);
                case DialogQuestionTutorialStepData dialogQuestionStepData:
                    return new DialogQuestionTutorialStep(dialogQuestionStepData, _dialogController);
                case DialogExitTutorialStepData dialogExitStepData:
                    return new DialogExitTutorialStep(dialogExitStepData, _dialogController);
            }

            throw new ArgumentException();
        }
    }
}