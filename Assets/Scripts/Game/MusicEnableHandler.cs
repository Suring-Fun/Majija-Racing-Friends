using System.Collections;
using UnityEngine;

public class MusicEnableHandler : MonoBehaviour
{
    System.Action m_destuction;

    private AudioSource m_source;

    public float Duration = 0.5f;

    public string SettingName = "EnableMusic";

    void Awake()
    {
        m_source = GetComponent<AudioSource>();
        m_source.enabled = false;

        GameStartCondition c;
        (c = FindObjectOfType<GameStartCondition>()).GameStarted += EnableMusic;
        m_destuction = () => c.GameStarted -= EnableMusic;
    }

    private void EnableMusic()
    {
        StartCoroutine(C());

        IEnumerator C()
        {
            yield return new WaitForSeconds(Duration);
            m_source.enabled = PlayerSettings.Instance[SettingName];
        }
    }

    void OnDestroy()
    {
        m_destuction?.Invoke();
    }
}
