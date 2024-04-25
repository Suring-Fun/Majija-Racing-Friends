using System;
using UnityEngine;

public class LungePrize : IPrize
{
    public int Count { get; private set; }

    private Sprite[] m_iconPerCount;

    public Sprite Icon => m_iconPerCount[Mathf.Clamp(Count - 1, 0, m_iconPerCount.Length - 1)];

    public bool IsApplyable => Count > 0 && !m_inUse;

    public bool IsReplaceable => !m_inUse;

    public float Amount { get; private set; } = 1f;

    private float m_amountPerSecond;

    private float m_flySpeed;

    public IPrize.PrizeApplyMode ApplyMode => IPrize.PrizeApplyMode.TakeAim;

    public event Action<IPrize> PrizeChanged;

    private bool m_inUse;

    private Vector2 m_globalDirection;
    private Vector2 m_lastVelocity;

    private Transform m_car;
    private Movenment m_moveable;
    private ShockableCar m_shockable;
    private SafeEffect m_safeEffect;
    private GameObject m_audioInstance;

    private float m_audioLifeTime;

    // private float m_angleFrom;
    // private float m_angleTo;

    public LungePrize(Transform car, Sprite[] icons, float radius, float duration, int count, GameObject audioInstance, float audioLifeTime)
    {
        m_car = car;
        m_moveable = car.GetComponent<Movenment>();
        m_shockable = car.GetComponent<ShockableCar>();
        m_safeEffect = m_car.GetComponent<SafeEffect>();

        m_iconPerCount = icons;
        m_amountPerSecond = 1f / duration;
        m_flySpeed = radius / duration;

        Count = count;

        m_audioInstance = audioInstance;
        m_audioLifeTime = audioLifeTime;
    }

    public void Apply(Vector2 direction)
    {
        m_inUse = true;
        var rigidbody = m_moveable.Rigidbody2D;

        if (m_moveable.FreeFly <= 0)
        {
            m_lastVelocity = rigidbody.velocity;
        }
        else
        {
            m_lastVelocity = default;
        }


        m_shockable.IgnoreCollisionShocks++;
        m_moveable.FreeFly++;

        m_globalDirection = m_car.up * direction.y + m_car.right * direction.x;
        m_globalDirection.Normalize();

        m_shockable.AbortShocking();
        m_safeEffect.AbortSafeEffect();
        // m_angleFrom = rigidbody.rotation;
        // m_angleTo = Mathf.Atan2(-m_globalDirection.x, m_globalDirection.y) * Mathf.Rad2Deg;
        // m_angleTo = Mathf.LerpAngle(m_angleFrom, m_angleTo, Vector2.Dot(m_car.up, m_globalDirection));
        PrizeChanged?.Invoke(this);

        GameObject.Destroy(GameObject.Instantiate(m_audioInstance, m_car, false), m_audioLifeTime);
    }

    public float PreviewDistance(Vector2 direction)
    {
        return float.PositiveInfinity;
    }

    public IPrize.UpdateResult Update(float delata)
    {
        if (m_inUse)
        {
            Amount -= m_amountPerSecond * delata;
            if (Amount <= 0f)
            {
                m_inUse = false;
                Count--;

                m_moveable.Rigidbody2D.velocity = default;
                m_shockable.IgnoreCollisionShocks--;
                m_moveable.FreeFly--;
                Amount = 1f;

                // m_moveable.Rigidbody2D.rotation = m_angleTo;

                if (Count <= 0)
                    return IPrize.UpdateResult.PrizeRetired;
            }
            else
            {
                m_moveable.Rigidbody2D.velocity = m_globalDirection * m_flySpeed + m_lastVelocity;
                // m_moveable.Rigidbody2D.rotation = Mathf.LerpAngle(m_angleTo, m_angleFrom, Amount);
            }

            PrizeChanged?.Invoke(this);
        }

        return IPrize.UpdateResult.None;
    }

    public Vector2 PreviewDirection(Vector2 direction)
    {
        return direction.normalized;
    }
}