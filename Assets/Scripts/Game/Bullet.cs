using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [field: SerializeField]
    public float Lifetime { get; private set; } = 30f;
    public float Speed { get; private set; }

    [field: SerializeField]
    public AudioSource ReflectionAudioSource { get; private set; }

    private Rigidbody2D m_rigidbody;
    private PathData m_path;
    private Vector2 m_totalVelocity;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_path = FindObjectOfType<PathData>();
    }

    public void Init(Vector2 direction, Vector2 additionalSpeed, float speed)
    {
        m_totalVelocity = direction * speed + additionalSpeed;
        Speed = speed;

        if (m_totalVelocity.magnitude < Speed)
        {
            m_totalVelocity = direction * Speed;
        }

        if (Lifetime > 0f)
        {
            IEnumerator KillCoroutine()
            {
                yield return new WaitForSeconds(Lifetime);
                IPreDestroying.NotifyObjectAboutDeath(gameObject);
                Destroy(gameObject);
            }

            StartCoroutine(KillCoroutine());
        }
    }

    private void FixedUpdate()
    {
        m_rigidbody.velocity = m_totalVelocity;

        if (m_path.EdgesAreSolid)
        {
            var p = m_path.GetNearestPoint(m_rigidbody.position);
            float t = p.radius + m_path.DistanceToWater + m_path.DistanceToDeepWater;
            var x = p.point - m_rigidbody.position;

            if ((x).magnitude > t)
            {
                x.Normalize();
                m_rigidbody.position = p.point - x * t;

                if (Vector2.Dot(m_totalVelocity, x) < 0f)
                {
                    m_rigidbody.velocity = m_totalVelocity = Vector2.Reflect(m_totalVelocity, x);
                    if (ReflectionAudioSource)
                        ReflectionAudioSource.Play();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        IPreDestroying.NotifyObjectAboutDeath(gameObject);
        Destroy(gameObject);
    }
}