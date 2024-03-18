using System;
using UnityEngine;

public abstract class AIAction : MonoBehaviour
{
    protected PrizeHost Host { get; private set; }

    protected Movenment Moveable { get; private set; }

    protected Transform Car { get; private set; }

    protected PathData PathData { get; private set; }

    void Awake()
    {
        Host = GetComponentInParent<PrizeHost>();

        Moveable = GetComponentInParent<Movenment>();
        Car = Moveable.transform;

        PathData = FindObjectOfType<PathData>();
    }

    public abstract bool CanHandle();

    public abstract Func<float, bool> CreateHandler();
}

public class AimAIAction<T> : AIAction
{
    public float LookForward = 10f;

    [field: SerializeField]
    public float Radius { get; private set; } = 7f;


    [field: SerializeField]
    public Vector2 DefaultDirection { get; private set; }

    [field: SerializeField]
    public bool DefaultDirectionMathToRoadDirection { get; private set; }

    public override bool CanHandle()
    {
        return Host.MainPrize is T;
    }

    public override Func<float, bool> CreateHandler()
    {
        float time = 1f;
        Vector2 direction = default;

        return deltaTime =>
        {
            Host.DisableApplyView();

            if (Host.MainPrize is null)
                return false;

            if (!Host.MainPrizeAccessed)
                return true; // Wait until access.



            var moveables = FindObjectsOfType<Movenment>();

            bool someSelected = false;
            float distance = float.PositiveInfinity;

            for (int x = 0; x < moveables.Length; ++x)
            {
                Movenment movenment = moveables[x];
                if (movenment == Moveable)
                {
                    continue;
                }

                Vector2 globalDirection = (Vector2)(movenment.transform.position - Car.position);
                float d = globalDirection.magnitude;

                if (d < Radius && d < distance)
                {
                    distance = d;
                    direction = new Vector2(
                        Vector2.Dot(Car.right, globalDirection),
                        Vector2.Dot(Car.up, globalDirection)
                    );
                    someSelected = true;
                }
            }

        Unselected:
            if (someSelected)
            {
                direction.Normalize();
                float _01 = Host.MainPrize.PreviewDistance(direction);

                if (_01 <= float.Epsilon)
                {
                    someSelected = false;
                    goto Unselected;
                }

                if (float.IsFinite(_01))
                {
                    _01 = Mathf.Clamp01(distance / _01);
                    direction *= _01;
                }
            }
            else
            {
                if (DefaultDirectionMathToRoadDirection)
                {
                    (_, _, _, direction) = PathData.GetLocationAtTrack(Car.position + Car.up * LookForward);
                    direction = new Vector2(
                        Vector2.Dot(Car.right, direction),
                        Vector2.Dot(Car.up, direction)
                    );
                }
                else
                    direction = DefaultDirection;
            }

            time -= deltaTime;
            if (time < 0f)
            {
                Host.ApplyPrize(direction);
                return false;
            }

            Host.EnableApplyPreview(direction);
            return true;
        };
    }
}