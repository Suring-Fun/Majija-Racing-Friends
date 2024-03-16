using UnityEngine;
using UnityEngine.UI;

public class OpenSceneClickHandler : MonoBehaviour
{
    private Button m_button;

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnPointerClick);
    }
    public string SceneToOpen;

    public bool RestartCurrentScene;
    private void OnPointerClick()
    {
        var mng = SceneTransitionManager.Main;
        mng.LaunchSceneTransition(RestartCurrentScene ? mng.CurrentScene : SceneToOpen);
    }
}
