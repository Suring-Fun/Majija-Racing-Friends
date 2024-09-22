using System;
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

    public bool IsDirty { get; private set; } = false;

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

            IsDirty = true;
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

    [Obsolete("Cutscenes is canceled.")]
    public bool EnableCutsceneFX
    {
        get => this["EnableCutsceneFX"];
        set => this["EnableCutsceneFX"] = value;
    }

    public bool EnableHelp
    {
        get => this["EnableHelp"];
        set => this["EnableHelp"] = value;
    }

    public void LoadChanges()
    {
        var saves = SaveStorage.Data;
        EnableMusic = saves.EnableMusic;
        EnableSound = saves.EnableSound;
        EnableHelp = saves.EnableHelp;
        IsDirty = false;
    }

    public void SaveChanges()
    {
        var saves = SaveStorage.Data;
        saves.EnableMusic = EnableMusic;
        saves.EnableSound = EnableSound;
        saves.EnableHelp = EnableHelp;
        SaveStorage.Save();
        IsDirty = false;
    }


}
