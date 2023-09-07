using System;
using GameLoop;
using GameLoop.EndlessMode;
using GameLoop.Tutorial;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GameInitialization
{
    public class GameLoopsCollection : MonoBehaviour
    {
        private const string DefaultEditorGameLoopKey = "default_editor_game_loop";

        [SerializeField] private EndlessModeGameLoop _endlessMode;
        [SerializeField] private TutorialLoop _tutorial;

        public AbstractGameLoop GetGameLoopToRun(GameLoopType? gameLoopType)
        {
            if (gameLoopType != null)
                return GetGameLoop(gameLoopType.Value);
            if (Application.isEditor)
                return GetDefaultEditorGameLoop();

            throw new ArgumentException("GameLoop was not set");
        }

        public AbstractGameLoop GetGameLoop(GameLoopType gameLoopType)
        {
            switch (gameLoopType)
            {
                case GameLoopType.EndlessMode:
                    return _endlessMode;
                case GameLoopType.Tutorial:
                    return _tutorial;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameLoopType), gameLoopType, null);
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