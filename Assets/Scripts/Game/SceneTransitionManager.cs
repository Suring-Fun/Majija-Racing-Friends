using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Main { get; private set; }

    public SceneTransitionManager() => Main = this;

    [field: SerializeField]
    public GameObject LoadingScreen { get; private set; }

    [field: SerializeField]
    public Animator Curtains { get; private set; }

    [field: SerializeField]
    public string CurtainsEntrence = "Enterence";

    [field: SerializeField]
    public string CurtainsExit = "Exit";

    [field: SerializeField]
    public float TransitionTime = 0.5f;


    private bool m_isLoading = false;
    private string m_lastLoadedScene = null;

    public string CurrentScene => m_lastLoadedScene;

    IEnumerator LoadingCoroutine(string scene)
    {
        m_isLoading = true;
        Curtains.gameObject.SetActive(true);
        Curtains.Play(CurtainsEntrence, 0);

        yield return new WaitForSeconds(TransitionTime);
        yield return null;
        yield return null;

        Curtains.gameObject.SetActive(false);
        LoadingScreen.SetActive(true);

        if (m_lastLoadedScene is not null)
            yield return SceneManager.UnloadSceneAsync(m_lastLoadedScene);

        Resources.UnloadUnusedAssets();


        bool adIsShowing = false;

        System.Action adShowingHandler = () => adIsShowing = true;
        YandexGame.onAdNotification += adShowingHandler;

        System.Action adClosedHandler = null;  
        adClosedHandler = () => adIsShowing = false;    
        YandexGame.CloseFullAdEvent += adClosedHandler;

        YandexGame.FullscreenShow();

        while (adIsShowing)
            yield return null;

        YandexGame.CloseFullAdEvent -= adClosedHandler;
        YandexGame.onAdNotification -= adShowingHandler;

        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        m_lastLoadedScene = scene;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));

        yield return null;
        yield return null;

        LoadingScreen.SetActive(false);

        Curtains.gameObject.SetActive(true);
        Curtains.Play(CurtainsExit, 0);

        yield return new WaitForSeconds(TransitionTime);

        Curtains.gameObject.SetActive(false);
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
