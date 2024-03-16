using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManagerKicker : MonoBehaviour
{
    [field: SerializeField]
    public string SceneToKickFor { get; private set; }

    void Awake()
    {
        SceneTransitionManager.Main.LaunchSceneTransition(SceneToKickFor);
    }
}
