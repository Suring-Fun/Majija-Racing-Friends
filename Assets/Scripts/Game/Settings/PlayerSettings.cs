using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    private HashSet<string> m_settingsDisabled = new();

    public event Action<string, bool> SettingChanged;

    public bool this[string key]
    {
        get => !m_settingsDisabled.Contains(key);
        set
        {
            if (value)
            {
                if (m_settingsDisabled.Remove(key))
                    SettingChanged?.Invoke(key, true);

            }
            else
            {
                if (m_settingsDisabled.Add(key))
                    SettingChanged?.Invoke(key, false);
            }
        }
    }

    public bool EnableMusic
    {
        get => this["EnableMusic"];
        set => this["EnableMusic"] = value;
    }

    public bool EnableSound
    {
        get => this["EnableSound"];
        set => this["EnableSound"] = value;
    }
    public bool EnableCutsceneFX
    {
        get => this["EnableCutsceneFX"];
        set => this["EnableCutsceneFX"] = value;
    }


}
