using System;
using JetBrains.Annotations;
using UnityEngine;

public class SpeedUpPrize : IPrize
{
    public Sprite Icon { get; }

    public bool IsApplyable { get; private set; } = true;

    public float Amount { get; private set; } = 1f;

    public float AmountDecreaseSpeed { get; }

    public float MoveMultFactor { get; }
    public float RotateMultFactor { get; }

    public IPrize.PrizeApplyMode ApplyMode => IPrize.PrizeApplyMode.JustApply;

    private Action m_undoAction;

    private bool m_isRunning = false;

    public event Action<IPrize> PrizeChanged;

    private Transform m_car;
    private ShockableCar m_shockable;

    public SpeedUpPrize(Transform car, Sprite icon, float amountDecrease01PerSecond, float moveMultFactor, float rotateMultFactor)
    {
        m_car = car;
        Icon = icon;
        AmountDecreaseSpeed = amountDecrease01PerSecond;
        MoveMultFactor = moveMultFactor;
        RotateMultFactor = rotateMultFactor;
        m_shockable = car.GetComponent<ShockableCar>();
    }

    public void Apply(Vector2 direction)
    {
        if (!IsApplyable)
            return;

        IsApplyable = false;

        m_shockable.AbortShocking();
        var movenment = m_car.GetComponent<Movenment>();
        var shockable = m_car.GetComponent<ShockableCar>();

        var multSpeedUndoAction = movenment.MultSpeed(MoveMultFactor, RotateMultFactor);
        shockable.IgnoreCollisionShocks++;

        m_undoAction = () =>
        {
            shockable.IgnoreCollisionShocks--;
            multSpeedUndoAction.Invoke();
        };

        m_isRunning = true;
        PrizeChanged?.Invoke(this);
    }

    public IPrize.UpdateResult Update(float delata)
    {
        if (!m_isRunning)
            return IPrize.UpdateResult.None;

        Amount -= AmountDecreaseSpeed * delata;

        var result = IPrize.UpdateResult.None;

        if (Amount < 0f)
        {
            m_isRunning = false;
            m_undoAction();
            m_undoAction = null;

            result = IPrize.UpdateResult.PrizeRetired;
        }

        PrizeChanged?.Invoke(this);
        return result;
    }

    public float PreviewDistance(Vector2 direction)
    {
        return float.PositiveInfinity;
    }
}
