
using System;
using UnityEngine;

public class AIPrizeController : MonoBehaviour
{

    public float MinPause = 3f;
    public float MaxPause = 10f;

    private float m_time;

    private bool m_running;

    public PrizeHost Host { get; private set; }

    private Func<float, bool> m_handler;

    void Start()
    {
        (Host = GetComponentInParent<PrizeHost>()).PrizeChanged += PH;
        PH(Host);
    }

    private void PH(PrizeHost host)
    {
        ReRun();
    }

    private void ReRun()
    {
        m_running = Host.Prize is object;
        m_time = UnityEngine.Random.Range(MinPause, MaxPause);
    }

    void FixedUpdate()
    {
        if (m_handler is object)
        {
            if (!m_handler(Time.deltaTime))
            {
                m_handler = null;
                ReRun();
            }
        }
    }

    void Update()
    {
        if (m_running && m_handler is null)
        {
            m_time -= Time.deltaTime;

            if (m_time < 0f)
            {
                m_running = false;
                foreach (var aiAction in GetComponentsInChildren<AIAction>())
                {
                    if (aiAction.CanHandle())
                    {
                        m_handler = aiAction.CreateHandler();
                        break;
                    }
                }
            }
        }
    }
}
