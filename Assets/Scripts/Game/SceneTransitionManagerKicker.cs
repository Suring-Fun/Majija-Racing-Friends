using System.Collections;
using UnityEngine;

public class SceneTransitionManagerKicker : MonoBehaviour
{
    [field: SerializeField]
    public string SceneToKickFor { get; private set; }

    [field: SerializeField]
    public bool UsePlatformAPI { get; private set; } = true;

    IEnumerator Start()
    {
        if (UsePlatformAPI)
        {
            while (!SaveStorage.IsInited)
                yield return null;

            SaveStorage.Load();
            PlayerProgress.Main.LoadChanges();
            PlayerSettings.Instance.LoadChanges();
        }

        SceneTransitionManager.Main.LaunchSceneTransition(SceneToKickFor);
    }
}
