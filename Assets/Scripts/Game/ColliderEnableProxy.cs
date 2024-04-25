using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEnableProxy : MonoBehaviour
{
    public struct Lock
    {

        private ColliderEnableProxy m_proxy;

        private bool m_locked;

        public bool Enabled
        {
            get => !Locked;
            set => Locked = !value;
        }

        public bool Locked
        {
            get => m_locked;

            set
            {
                if (m_locked != value)
                {
                    m_proxy.Locks += value ? 1 : -1;
                    m_locked = value;
                }
            }
        }

        public Lock(ColliderEnableProxy proxy)
        {
            m_proxy = proxy;
            m_locked = false;
        }
    }

    private Collider2D m_collider;

    private int m_locks;

    public int Locks
    {
        get => m_locks;
        set
        {
            m_locks = value;
            m_collider.enabled = value <= 0;
        }
    }

    void Awake()
    {
        m_collider = GetComponent<Collider2D>();
    }

    public Lock CreateLock() => new(this);
}
