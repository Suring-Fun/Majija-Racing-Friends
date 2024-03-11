using UnityEngine;

public class SimplePauseMaker : MonoBehaviour
{
    public GameObject Overlay;
    public string MenuSceneName = "MenuScene";

    public void Pause() {
        Overlay.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume() {
        Overlay.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart() {
        Time.timeScale = 1f;
        SceneTransitionManager.Main.LaunchSceneTransition(SceneTransitionManager.Main.CurrentScene);
    }

    public void ExitToMenu() {
        Time.timeScale = 1f;
        SceneTransitionManager.Main.LaunchSceneTransition(MenuSceneName);
    }
}
