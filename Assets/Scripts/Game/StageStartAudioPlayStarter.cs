using UnityEngine;

public class StageStartAudioPlayStarter : MonoBehaviour
{
    private AudioSource m_source;
    private void Start()
    {
        m_source = GetComponent<AudioSource>();
        FindObjectOfType<GameStartCondition>().CountDownStarted += () => m_source.Play();
    }
}
