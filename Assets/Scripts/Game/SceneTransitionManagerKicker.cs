using System.Collections;
using UnityEngine;
using YG;

public class SceneTransitionManagerKicker : MonoBehaviour
{
    [field: SerializeField]
    public string SceneToKickFor { get; private set; }

    IEnumerator Start()
    {
        while(!YandexGame.SDKEnabled)
            yield return null;

        PlayerProgress.Main.LoadChanges();
        PlayerSettings.Instance.LoadChanges();

        SceneTransitionManager.Main.LaunchSceneTransition(SceneToKickFor);
    }
}
