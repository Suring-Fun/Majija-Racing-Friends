using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningCarAudioController : MonoBehaviour
{
    public float MinStayPitch = 0.3f, MinRunningPitch = 0.6f, MinRotating = 0.65f;
    public float MaxStayPitch = 0.3f, MaxRunningPitch = 0.6f, MaxRotating = 0.65f;
    private float StayPitch = 0.3f, RunningPitch = 0.6f, Rotating = 0.7f;
    private AudioSource m_audio;
    private Movenment m_movenment;

    // Start is called before the first frame update
    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        m_movenment = GetComponentInParent<Movenment>();

        StayPitch = UnityEngine.Random.Range(MinStayPitch, MaxStayPitch);
        RunningPitch = UnityEngine.Random.Range(MinRunningPitch, MaxRunningPitch);
        Rotating = UnityEngine.Random.Range(MinRotating, MaxRotating);
    }

    // Update is called once per frame
    void Update()
    {
        float currentPitch = Mathf.Lerp(StayPitch, Mathf.Lerp(RunningPitch, Rotating, Mathf.Abs(m_movenment.RotationDirection)), m_movenment.CurrentSpeed / m_movenment.MaxSpeed);
        m_audio.pitch = currentPitch * m_movenment.EnginePower;
    }
}
