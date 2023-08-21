using System;
using Cysharp.Threading.Tasks;
using GameLoop.EndlessMode;
using GameLoop.Tutorial;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GameLoop
{
    public class GameLoopRunner : MonoBehaviour
    {
        private const string DefaultEditorGameLoopKey = "default_editor_game_loop";

        [SerializeField] private EndlessModeGameLoop _endlessMode;
        [SerializeField] private TutorialLoop _tutorial;

        public static GameLoopType? GameLoopToRun;

        private void Awake()
        {
            AbstractGameLoop gameLoop = GetGameLoopToRun();
            gameLoop.Setup();
        }

        private void Start()
        {
            AbstractGameLoop gameLoop = GetGameLoopToRun();

            gameLoop.Run().Forget();
        }

        private AbstractGameLoop GetGameLoopToRun()
        {
            if (GameLoopToRun == null)
            {
                if (Application.isEditor)
                    return GetDefaultEditorGameLoop();

                throw new ArgumentException("GameLoop was not set");
            }

            return GetGameLoop(GameLoopToRun.Value);
        }

        private AbstractGameLoop GetGameLoop(GameLoopType gameType)
        {
            switch (gameType)
            {
                case GameLoopType.EndlessMode:
                    return _endlessMode;
                case GameLoopType.Tutorial:
                    return _tutorial;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null);
            }
        }

        private AbstractGameLoop GetDefaultEditorGameLoop()
        {
#if UNITY_EDITOR
            string gameLoopTypeString = EditorPrefs.GetString(DefaultEditorGameLoopKey);

            if (Enum.TryParse(gameLoopTypeString, out GameLoopType gameLoopType))
                return GetGameLoop(gameLoopType);

            throw new ArgumentException("GameLoop was not set");
#else
            return null;
#endif
        }

        [Button]
        private void SetDefaultEditorGameLoop(GameLoopType gameType)
        {
#if UNITY_EDITOR
            EditorPrefs.SetString(DefaultEditorGameLoopKey, gameType.ToString());
#endif
        }
    }
}