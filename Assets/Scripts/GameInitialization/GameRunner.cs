using Cysharp.Threading.Tasks;
using GameLoop;
using GameSave;
using UnityEngine;

namespace GameInitialization
{
    public class GameRunner : MonoBehaviour
    {
        public static GameLoopType? GameLoopToRun;
        public static GameData CurrentSaveToLoad;

        [SerializeField] private GameLoopsCollection _gameLoopRunner;

        private AbstractGameLoop _currentGameLoop;

        private void Awake()
        {
            _currentGameLoop = _gameLoopRunner.GetGameLoopToRun(GameLoopToRun);

            if (CurrentSaveToLoad == null)
                _currentGameLoop.SetupEmptyGame();
            else
                _currentGameLoop.SetupFromSavedGame(CurrentSaveToLoad);
        }

        private void Start()
        {
            _currentGameLoop.Run().Forget();
        }
    }
}