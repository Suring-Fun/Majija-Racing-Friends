using UnityEngine;
using UnityEngine.EventSystems;

public class StageLaunchButton : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public StageSelectionManager Manager { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneTransitionManager.Main.LaunchSceneTransition(Manager.Info.SceneName);
    }
}
