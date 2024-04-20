using System;
using UnityEngine;
using YG;

public class PlayerProgress : MonoBehaviour
{
    [field: SerializeField]
    private int m_playerLevel;

    public static PlayerProgress Main { get; private set; }

    public int PlayerLevel
    {
        get => m_playerLevel;
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

    public void LoadChanges() {
        PlayerLevel = YandexGame.savesData.currentPlayerLevel;
    }

    public void SaveChanges()
    {
        YandexGame.savesData.currentPlayerLevel = PlayerLevel;
        YandexGame.SaveProgress();
    }
}
