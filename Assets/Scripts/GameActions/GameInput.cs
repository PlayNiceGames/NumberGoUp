using Cysharp.Threading.Tasks;
using GameConsumables;
using GameLoop;
using UnityEngine;

namespace GameActions
{
    public class GameInput : MonoBehaviour
    {
        [SerializeField] private BoardGameLoop _boardLoop;
        [SerializeField] private GameButton _exitButton;
        [SerializeField] private GameRewindButton _rewindButton;

        private UniTaskCompletionSource<GameInputActionType> _actionTask;
        
        public async UniTask<GameInputActionType> WaitForAction()
        {
            while (true)
            {
                _actionTask = new UniTaskCompletionSource<GameInputActionType>();
                
                UniTask boardInputTask = _boardLoop.ProcessAction();
                UniTask backButtonClickTask = _exitButton.WaitForClick();
                UniTask rewindButtonClickTask = _rewindButton.WaitForClick();

                await UniTask.WhenAny(boardInputTask, backButtonClickTask, rewindButtonClickTask);

                return GameInputActionType.Board;
            }
        }
    }
}