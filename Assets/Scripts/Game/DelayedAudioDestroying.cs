using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAudioDestroying : MonoBehaviour, IPreDestroying
{
    [field: SerializeField]
    public float Delay {get; private set; } = 8f;

    public void OnNotifiedObjectAboutDeath()
    {
        transform.parent = null;
        AudioSource sourceToPlay = GetComponent<AudioSource>();
        sourceToPlay.Play();
        Destroy(gameObject, Delay);
    }
}
