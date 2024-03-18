using System;
using UnityEngine;

public class ShootPrize : IPrize
{
    private Sprite[] m_iconPerCount;

    public Sprite Icon => m_iconPerCount[Mathf.Clamp(Count - 1, 0, m_iconPerCount.Length - 1)];


    public int Count { get; private set; }

    public Bullet BulletPrefab { get; private set; }

    public bool IsApplyable => Count > 0;

    public bool IsReplaceable => true;

    public float Amount => 1f;

    public IPrize.PrizeApplyMode ApplyMode => IPrize.PrizeApplyMode.TakeAim;

    public event Action<IPrize> PrizeChanged;

    private Transform m_car;
    private Movenment m_moveable;

    private float m_speed;

    public ShootPrize(Transform car, Sprite[] icons, float speed, int count, Bullet prefab)
    {
        Count = count;
        m_car = car;
        m_moveable = car.GetComponent<Movenment>();
        m_iconPerCount = icons;
        m_speed = speed;
        BulletPrefab = prefab;
    }


    public void Apply(Vector2 direction)
    {
        var bullet = UnityEngine.Object.Instantiate(BulletPrefab, m_car.position, Quaternion.identity);
        Physics2D.IgnoreCollision(m_car.GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
        bullet.Init(
            (m_car.up * direction.y + m_car.right * direction.x).normalized,
            m_moveable.Rigidbody2D.velocity,
            m_speed
            );
        Count--;

        PrizeChanged?.Invoke(this);
    }

    public float PreviewDistance(Vector2 direction)
    {
        return float.PositiveInfinity;
    }

    public IPrize.UpdateResult Update(float delata)
    {
        if (Count <= 0)
            return IPrize.UpdateResult.PrizeRetired;

        return IPrize.UpdateResult.None;
    }
}