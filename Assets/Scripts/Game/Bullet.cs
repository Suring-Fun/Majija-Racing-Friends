using UnityEngine;

public class Bullet : MonoBehaviour
{
    [field: SerializeField]
    public float Lifetime { get; private set; } = 30f;
    public float Speed { get; private set; }

    private Rigidbody2D m_rigidbody;
    private Vector2 m_direction;
    private Vector2 m_addSpeed;



    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction, Vector2 additionalSpeed, float speed)
    {
        m_direction = direction;
        m_addSpeed = additionalSpeed;
        Speed = speed;

        if (Lifetime > 0f)
            Destroy(gameObject, Lifetime);
    }

    private void FixedUpdate()
    {
        m_rigidbody.velocity = m_direction * Speed + m_addSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        Destroy(gameObject);
    }
}