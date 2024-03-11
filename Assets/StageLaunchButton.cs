using UnityEngine;
using UnityEngine.EventSystems;

public class StageLaunchButton : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public StageSelectionManager Manager { get; private set; }

    [field: SerializeField] public GameObject Overlay { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        var info = Manager.Info;
        if (info.LevelRequired <= PlayerProgress.Main.PlayerLevel)
            SceneTransitionManager.Main.LaunchSceneTransition(info.SceneName);
        else
            Overlay.SetActive(true);
    }
}
