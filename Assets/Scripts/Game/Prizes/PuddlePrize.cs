using System;
using UnityEngine;

public class PuddlePrize : IPrize
{

    public int Count { get; private set; }

    public OilBullet OilBulletPrefab { get; private set; }

    private Sprite[] m_oilPerIcon;

    public Sprite Icon => m_oilPerIcon[Mathf.Clamp(Count - 1, 0, m_oilPerIcon.Length - 1)];

    public bool IsApplyable => Count > 0;

    public bool IsReplaceable => true;

    public float Amount => 1f;

    public IPrize.PrizeApplyMode ApplyMode => IPrize.PrizeApplyMode.TakeAim;

    public float MinDistance { get; private set; } = 5f;

    public float MaxDistance { get; private set; } = 15f;

    public float TimeToFly { get; private set; } = 0.3f;

    Transform m_car;

    public event Action<IPrize> PrizeChanged;

    public PuddlePrize(Transform car, Sprite[] icons, OilBullet bullet, int count, float minDistance, float maxDistance, float flyTime)
    {
        m_car = car;
        m_oilPerIcon = icons;
        OilBulletPrefab = bullet;
        Count = count;

        MinDistance = minDistance;
        MaxDistance = maxDistance;
        TimeToFly = flyTime;
    }

    public void Apply(Vector2 direction)
    {
        Count--;
        var puddleBullet = UnityEngine.Object.Instantiate(OilBulletPrefab, m_car.position, Quaternion.identity);

        var normalizedDirection = direction.normalized;
        Vector2 globalOffset = m_car.up * normalizedDirection.y + m_car.right * normalizedDirection.x;
        puddleBullet.Init(
            m_car.transform.position, 
            (Vector2)m_car.position + globalOffset * PreviewDistance(direction) + 
            (Vector2)m_car.up * m_car.GetComponent<Movenment>().CurrentSpeed * TimeToFly, 
            TimeToFly
            );
        PrizeChanged?.Invoke(this);
    }

    public float PreviewDistance(Vector2 direction)
    {
        return Mathf.Lerp(MinDistance, MaxDistance, Vector2.Dot(Vector2.up, direction.normalized)) * direction.magnitude;
    }

    public IPrize.UpdateResult Update(float delata)
    {
        if (Count <= 0)
            return IPrize.UpdateResult.PrizeRetired;

        return IPrize.UpdateResult.None;
    }
}