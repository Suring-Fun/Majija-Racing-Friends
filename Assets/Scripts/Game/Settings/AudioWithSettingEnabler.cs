using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioWithSettingEnabler : MonoBehaviour
{
    private AudioSource m_AudioSource;

    [field: SerializeField]
    public string SettingName { get; private set; }

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayerSettings.Instance.SettingChanged += OnSettingChanged;
        OnSettingChanged(SettingName, PlayerSettings.Instance[SettingName]);
    }

    private void OnDisable()
    {
        PlayerSettings.Instance.SettingChanged -= OnSettingChanged;
    }

    private void OnSettingChanged(string settingName, bool value)
    {
        if (settingName == SettingName)
            m_AudioSource.enabled = value;
    }
}
