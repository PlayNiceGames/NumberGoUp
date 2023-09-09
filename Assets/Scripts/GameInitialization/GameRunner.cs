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
            _currentGameLoop.Setup();
        }

        private void Start()
        {
            if (CurrentSaveToLoad == null)
                _currentGameLoop.RunNewGame();
            else
                _currentGameLoop.RunSavedGame(CurrentSaveToLoad);
        }
    }
}