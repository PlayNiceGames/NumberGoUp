using Cysharp.Threading.Tasks;
using GameInitialization;
using GameLoop;
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

    public static void LoadEndlessMode()
    {
        GameLoopRunner.GameLoopToRun = GameLoopType.EndlessMode;

        SceneManager.LoadScene(GameSceneIndex);
    }

    public static UniTask LoadEndlessModeAsync()
    {
        GameLoopRunner.GameLoopToRun = GameLoopType.EndlessMode;

        return LoadSceneAsync(GameSceneIndex);
    }

    public static void LoadTutorial()
    {
        GameLoopRunner.GameLoopToRun = GameLoopType.Tutorial;

        SceneManager.LoadScene(GameSceneIndex);
    }

    public static UniTask LoadTutorialAsync()
    {
        GameLoopRunner.GameLoopToRun = GameLoopType.Tutorial;

        return LoadSceneAsync(GameSceneIndex);
    }

    private static UniTask LoadSceneAsync(int sceneIndex) //TODO fix
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        return asyncOperation.ToUniTask();
    }
}