using UnityEngine;

public class ShockableCar : MonoBehaviour
{
    [field: SerializeField]
    public float ShockTime { get; private set; } = 2f;

    [field: SerializeField]
    public AnimationCurve SpeedDownCurve { get; private set; }

    [field: SerializeField]
    public AnimationCurve RotationSpeedDownCurve { get; private set; }


    [field: SerializeField]
    public Transform Graphics { get; private set; }

    [field: SerializeField]
    public float MinStaySpeed { get; private set; } = 1f;

    [field: SerializeField]
    public float RotationSpeed { get; private set; } = 180f;

    [field: SerializeField]
    public float CollisionShockedThreshold = 2f;

    [field: SerializeField]
    public int ShockLimit { get; private set; } = 2;

    [field: SerializeField]
    public AudioSource ShockAudio { get; private set; }

    [field: SerializeField]
    public float ShockPitchRange { get; private set; } = 0.1f;

    private int m_shocks = 0;

    public int IgnoreCollisionShocks
    {
        get => m_ignoreCollisionShocks;
        set
        {
            m_ignoreCollisionShocks = value;
            IgnoreCollisionShocksChanged?.Invoke(this);
        }
    }

    [field: SerializeField]
    public string ShockWithTagWhenIgnoreShocks = "Animal";

    private bool m_shocked;

    public bool IsShocked => m_shocked;

    private float m_timePassed;

    private Vector2 m_shockDirection;

    private float m_speedFixed;

    private float m_rotationFrom;

    private float m_rotationTo;

    private Movenment m_movenmnt;

    [field: SerializeField]
    public float TurnOffColliderTime { get; private set; } = 0.5f;

    private Collider2D m_collider;
    private int m_ignoreCollisionShocks;
    public event System.Action<ShockableCar> IgnoreCollisionShocksChanged;

    private void Awake()
    {
        m_movenmnt = GetComponent<Movenment>();
        m_collider = GetComponent<Collider2D>();

    }

    [ContextMenu("Shock Random")]
    public void ShockRandom()
    {
        Shock(Random.insideUnitCircle.normalized);
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {

        Vector2 normal = default;

        for (int x = 0; x < collision2D.contactCount; ++x)
            normal += collision2D.GetContact(x).normal;

        normal.Normalize();

        if (IgnoreCollisionShocks > 0)
        {
            if (collision2D.gameObject.tag == ShockWithTagWhenIgnoreShocks)
                collision2D.gameObject.GetComponent<ShockableCar>().Shock(-normal, collision2D.otherRigidbody.velocity.magnitude);
            return;
        }

        Vector2 reflected;

        if (Vector2.Dot(normal, transform.up) < 0f)
            reflected = Vector2.Reflect(transform.up, normal);
        else
            reflected = transform.up;

        if (collision2D.relativeVelocity.sqrMagnitude > CollisionShockedThreshold * CollisionShockedThreshold)
        {
            float otherVel = collision2D.rigidbody.velocity.magnitude;
            if (collision2D.otherRigidbody)
            {
                otherVel = Mathf.Max(collision2D.otherRigidbody.velocity.magnitude, otherVel);
            }
            Shock(reflected, otherVel);
        }
        else
        {
            // Bad idea to rotate hard.
            //m_movenmnt.Rigidbody2D.rotation = Mathf.Atan2(-reflected.x, reflected.y) * Mathf.Rad2Deg;
        }
    }

    public void Shock(Vector2 shockDirection, float speed = 0f)
    {
        m_timePassed = 0f;
        m_shockDirection = shockDirection;
        m_speedFixed = Mathf.Max(speed, MinStaySpeed);

        m_rotationFrom = Mathf.Atan2(-transform.up.x, transform.up.y) * Mathf.Rad2Deg;
        m_rotationTo = Mathf.Atan2(-shockDirection.x, shockDirection.y) * Mathf.Rad2Deg;
        if (!m_shocked)
            m_movenmnt.FreeFly++;
        m_shocked = true;
        m_shocks++;

        if (m_shocks >= ShockLimit)
            GetComponent<RescueableCar>().RunRescueProgram(m_movenmnt.FetchTrackingData());

        if (ShockAudio) {
            ShockAudio.pitch = Random.Range(1f - ShockPitchRange, 1f + ShockPitchRange);
            ShockAudio.Play();
        }
    }

    public void AbortShocking()
    {
        if (m_shocked)
        {
            Graphics.localEulerAngles = default;
            m_shocked = false;
            m_movenmnt.FreeFly--;
            m_collider.enabled = true;
            GetComponent<RescueableCar>().Run180RescueProgramIfRequired();
            m_shocks = 0;
        }
    }

    void FixedUpdate()
    {
        if (m_shocked)
        {
            m_timePassed += Time.deltaTime;
            m_collider.enabled = m_timePassed > TurnOffColliderTime;
            if (m_timePassed < ShockTime)
            {
                float time01 = m_timePassed / ShockTime;
                float angle = Mathf.LerpAngle(m_rotationFrom, m_rotationTo, time01);

                transform.eulerAngles = Vector3.forward * angle;
                m_movenmnt.Rigidbody2D.velocity =
                m_shockDirection * (m_speedFixed * SpeedDownCurve.Evaluate(time01));

                m_movenmnt.Rigidbody2D.rotation = angle;
                //float targetDelta = Mathf.DeltaAngle(Graphics.eulerAngles.z, 0);
                Graphics.localEulerAngles += Vector3.forward *
                    RotationSpeed * RotationSpeedDownCurve.Evaluate(time01) * Time.deltaTime;

            }
            else
            {
                Graphics.localEulerAngles = default;
                m_shocked = false;
                m_movenmnt.FreeFly--;
                m_movenmnt.ResetAllSpeeds();

                m_collider.enabled = true;

                GetComponent<RescueableCar>().Run180RescueProgramIfRequired();
                m_shocks = 0;
            }
        }
    }
}
