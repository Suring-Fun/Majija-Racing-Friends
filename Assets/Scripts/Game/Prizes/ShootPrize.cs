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
    private Movenment m_movenment;

    private float m_speed;

    public ShootPrize(Transform car, Sprite[] icons, float speed, int count, Bullet prefab)
    {
        Count = count;
        m_car = car;
        m_movenment = car.GetComponent<Movenment>();
        m_iconPerCount = icons;
        m_speed = speed;
        BulletPrefab = prefab;
    }


    public void Apply(Vector2 direction)
    {
        //direction = SelectDirectionAuto(direction);
        var bullet = UnityEngine.Object.Instantiate(BulletPrefab, m_car.position, Quaternion.identity);
        Physics2D.IgnoreCollision(m_car.GetComponent<CarColliderHub>().ToGetOrTakeHit, bullet.GetComponent<Collider2D>());
        bullet.Init(
            (m_car.up * direction.y + m_car.right * direction.x).normalized,
            m_movenment.Rigidbody2D.velocity,
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

    private Vector2 SelectDirectionAuto(Vector2 direction)
    {
        // BAD IDEA
        direction.Normalize();
        direction = m_car.up * direction.y + m_car.right * direction.x;

        Vector2 ConvertBackLocal(Vector2 dirGlobal)
            => new(Vector2.Dot(m_car.right, dirGlobal), Vector2.Dot(m_car.up, dirGlobal));

        var carMovenments = UnityEngine.Object.FindObjectsOfType<Movenment>();

        float max = 0f;
        Movenment selectedMovenment = null;

        // Step 1: select car with best angle.
        foreach (var movenment in carMovenments)
        {
            if (movenment == m_movenment)
                continue;

            Vector2 directionToTheCar = (movenment.transform.position - m_car.position).normalized;

            float currentCos = Vector2.Dot(direction, directionToTheCar);

            if (currentCos > max)
            {
                selectedMovenment = movenment;
                max = currentCos;
            }
        }

        if (!selectedMovenment)
            return ConvertBackLocal(direction);

        Vector2 shootDirection = ShootingHelper.GetShootVector2(
            m_car.position,
            selectedMovenment.transform.position,
            selectedMovenment.Rigidbody2D.velocity,
            m_speed
        );

        if (shootDirection == default)
            return ConvertBackLocal(direction);

        return ConvertBackLocal(shootDirection);
    }

    public Vector2 PreviewDirection(Vector2 direction)
    {
        return direction;
    }
}