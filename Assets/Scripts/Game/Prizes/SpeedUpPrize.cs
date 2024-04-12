using System;
using UnityEngine;

public class SpeedUpPrize : IPrize
{
    public Sprite Icon { get; }

    public bool IsApplyable { get; private set; } = true;

    public bool IsReplaceable { get; private set; } = true;

    public float Amount { get; private set; } = 1f;

    public float AmountDecreaseSpeed { get; }

    public float MoveMultFactor { get; }
    public float RotateMultFactor { get; }

    public float EnginePowerMultFactor { get; }

    public IPrize.PrizeApplyMode ApplyMode => IPrize.PrizeApplyMode.JustApply;

    private Action m_undoAction;

    private bool m_isRunning = false;

    public event Action<IPrize> PrizeChanged;

    private Transform m_car;
    private ShockableCar m_shockable;

    private GameObject m_audioPrefab;

    private float m_audioLivetime;

    public SpeedUpPrize(Transform car, Sprite icon, float amountDecrease01PerSecond, float moveMultFactor, float rotateMultFactor, float enginePowerMultFactor, GameObject audioPrefab, float audioPrefabLiveTime)
    {
        m_car = car;
        Icon = icon;
        
        MoveMultFactor = moveMultFactor;
        RotateMultFactor = rotateMultFactor;
        EnginePowerMultFactor = enginePowerMultFactor;

        AmountDecreaseSpeed = amountDecrease01PerSecond;
        
        m_shockable = car.GetComponent<ShockableCar>();

        m_audioPrefab = audioPrefab;
        m_audioLivetime = audioPrefabLiveTime;
    }

    public void Apply(Vector2 direction)
    {
        if (!IsApplyable)
            return;

        IsApplyable = false;
        IsReplaceable = false;

        m_shockable.AbortShocking();
        var movenment = m_car.GetComponent<Movenment>();
        var shockable = m_car.GetComponent<ShockableCar>();

        var multSpeedUndoAction = movenment.MultSpeed(MoveMultFactor, RotateMultFactor, EnginePowerMultFactor);
        shockable.IgnoreCollisionShocks++;

        m_undoAction = () =>
        {
            shockable.IgnoreCollisionShocks--;
            multSpeedUndoAction.Invoke();
        };

        m_isRunning = true;
        PrizeChanged?.Invoke(this);

        GameObject.Destroy(GameObject.Instantiate(m_audioPrefab, m_car, false), m_audioLivetime);

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

    public Vector2 PreviewDirection(Vector2 direction)
    {
        return Vector2.up;
    }
}
