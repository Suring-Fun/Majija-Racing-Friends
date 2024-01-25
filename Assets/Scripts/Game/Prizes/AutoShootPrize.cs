using System;
using UnityEngine;

public class AutoShootPrize : IPrize
{
    public Sprite Icon { get; private set; }

    public bool IsApplyable { get; private set; } = true;

    public float Amount => 1f;

    public IPrize.PrizeApplyMode ApplyMode => IPrize.PrizeApplyMode.JustApply;

    public event Action<IPrize> PrizeChanged;

    public AutoBullet Prefab { get; private set; }

    private Transform m_car;

    public AutoShootPrize(Transform car, Sprite icon, AutoBullet prefab)
    {
        m_car = car;
        Prefab = prefab;
        Icon = icon;
    }

    public void Apply(Vector2 direction)
    {
        IsApplyable = false;

        var inst = UnityEngine.Object.Instantiate(Prefab, m_car.position, Quaternion.identity);
        inst.Init(m_car);

        PrizeChanged?.Invoke(this);
    }

    public float PreviewDistance(Vector2 direction)
    {
        return float.PositiveInfinity;
    }

    public IPrize.UpdateResult Update(float delata)
    {
        if (!IsApplyable)
            return IPrize.UpdateResult.PrizeRetired;

        return IPrize.UpdateResult.None;
    }
}