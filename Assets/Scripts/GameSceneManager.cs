using Cysharp.Threading.Tasks;
using GameInitialization;
using GameLoop;
using GameSave;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    private const int MainMenuSceneIndex = 1;
    private const int GameSceneIndex = 2;

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneIndex);
    }

    public static void LoadEndlessMode(GameData savedGame = null)
    {
        GameRunner.GameLoopToRun = GameLoopType.EndlessMode;
        GameRunner.CurrentSaveToLoad = savedGame;

        SceneManager.LoadScene(GameSceneIndex);
    }

    public static UniTask LoadEndlessModeAsync()
    {
        GameRunner.GameLoopToRun = GameLoopType.EndlessMode;

        return LoadSceneAsync(GameSceneIndex);
    }

    public static void LoadTutorial()
    {
        GameRunner.GameLoopToRun = GameLoopType.Tutorial;

        SceneManager.LoadScene(GameSceneIndex);
    }

    public static UniTask LoadTutorialAsync()
    {
        GameRunner.GameLoopToRun = GameLoopType.Tutorial;

        return LoadSceneAsync(GameSceneIndex);
    }

    private static UniTask LoadSceneAsync(int sceneIndex) //TODO fix
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        return asyncOperation.ToUniTask();
    }
}