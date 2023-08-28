using GameLoop;
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

    public static void LoadTutorial()
    {
        GameLoopRunner.GameLoopToRun = GameLoopType.Tutorial;

        SceneManager.LoadScene(GameSceneIndex);
    }
}