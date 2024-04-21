using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSettingsButtonHandler : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Handle);
    }

    private void Handle()
    {
        if (PlayerSettings.Instance.IsDirty)
            PlayerSettings.Instance.SaveChanges();
    }
}
