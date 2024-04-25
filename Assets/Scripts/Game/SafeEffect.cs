using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphicsBlinker))]
public class SafeEffect : MonoBehaviour
{
    [field: SerializeField]
    public float DefaultSafeDuration { get; private set; } = 5f;

    [field: SerializeField]
    public float BlinkDuration { get; private set; } = 0.1f;

    [field: SerializeField]
    public ColliderEnableProxy DefenselessProxy { get; private set; }

    private ColliderEnableProxy.Lock m_lock;

    private GraphicsBlinker m_blinker;

    private float m_time = 0f;
    private int m_phase;

    void Awake()
    {
        m_blinker = GetComponent<GraphicsBlinker>();
        m_lock = DefenselessProxy.CreateLock();
    }

    void Update()
    {
        if (m_time > 0f)
        {
            m_time -= Time.deltaTime;
            m_lock.Locked = true;
            m_blinker.IsShown = (((int)(m_time / BlinkDuration) + m_phase) % 2) == 0;

        }
        else
        {
            m_lock.Locked = false;
            m_blinker.IsShown = true;
        }
    }

    public void RunSafeEffect()
    {
        RunSafeEffect(DefaultSafeDuration);
    }

    public void AbortSafeEffect()
    {
        m_time = 0f;
    }

    public void RunSafeEffect(float customDuration)
    {
        m_time = customDuration;
        m_phase = Random.value > 0.5f ? 1 : 0;
    }
}
