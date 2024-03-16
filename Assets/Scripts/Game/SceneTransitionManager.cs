using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Main { get; private set; }

    public SceneTransitionManager() => Main = this;

    [field: SerializeField]
    public GameObject Curtains { get; private set; }

    private bool m_isLoading = false;
    private string m_lastLoadedScene = null;

    public string CurrentScene => m_lastLoadedScene;

    IEnumerator LoadingCoroutine(string scene)
    {
        m_isLoading = true;
        Curtains.SetActive(true);

        if (m_lastLoadedScene is not null)
            yield return SceneManager.UnloadSceneAsync(m_lastLoadedScene);

        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        m_lastLoadedScene = scene;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));

        Curtains.SetActive(false);  
        m_isLoading = false;
    }

    public bool LaunchSceneTransition(string sceneName)
    {
        if (m_isLoading)
            return false;

        StartCoroutine(LoadingCoroutine(sceneName));
        return true;
    }
}
