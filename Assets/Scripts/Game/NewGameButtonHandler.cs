using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameButtonHandler : MonoBehaviour
{
    [field: SerializeField]
    public string SceneName { get; private set; } = "IntroComics";

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Handle);
    }

    private void Handle()
    {
        PlayerProgress.Main.PlayerLevel = 0;
        PlayerProgress.Main.SaveChanges();

        SceneTransitionManager.Main.LaunchSceneTransition(SceneName);
    }
}
