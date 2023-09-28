using System;
using GameActions;
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
        [SerializeField] private BoardInput _boardInput;
        [SerializeField] private BoardGameLoop _boardLoop;
        [SerializeField] private TileFactory _tileFactory;
        [SerializeField] private TutorialDialogController _dialogController;
        [SerializeField] private GameButton _exitButton;

        public TutorialStep CreateStep(TutorialStepData data)
        {
            switch (data)
            {
                case DelayTutorialStepData delayStepData:
                    return new DelayTutorialStep(delayStepData);
                case SetupBoardTutorialStepData setupBoardStepData:
                    return new SetupBoardTutorialStep(setupBoardStepData, _tileFactory, _board);
                case BoardTurnTutorialStepData boardTurnStepData:
                    return new BoardTurnTutorialStep(boardTurnStepData, _boardInput, _boardLoop, _exitButton);
                case DialogTutorialStepData dialogStepData:
                    return new DialogTutorialStep(dialogStepData, _dialogController);
                case DialogQuestionTutorialStepData dialogQuestionStepData:
                    return new DialogQuestionTutorialStep(dialogQuestionStepData, _dialogController, _exitButton);
                case DialogExitTutorialStepData dialogExitStepData:
                    return new DialogExitTutorialStep(dialogExitStepData, _dialogController, _exitButton);
            }

            throw new ArgumentException();
        }
    }
}