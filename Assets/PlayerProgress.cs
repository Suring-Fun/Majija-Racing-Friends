using System;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    [field: SerializeField]
    private int m_playerLevel;

    public static PlayerProgress Main { get; private set; }

    public int PlayerLevel
    {
        get => m_playerLevel; private
         set
        {
            m_playerLevel = value;
            ProgressChanged?.Invoke(this);
        }
    }

    public event Action<PlayerProgress> ProgressChanged;

    public PlayerProgress()
    {
        Main = this;
    }
}
