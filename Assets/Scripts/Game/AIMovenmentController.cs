using UnityEngine;

public class AIMovenmentController : MonoBehaviour
{
    public Movenment Movenment { get; private set; }

    public ShockableCar Shockable { get; private set; }

    public float SafeBoards = 2f;

    public float maxLerpFactor = 0.8f;

    public float DirectionScale = 1f;

    [field: Tooltip("1 left, 0 center, +1 right")]
    [field: Range(-1f, 1f)]
    public float PositionAtTheRoad = 0f; // -1 left, 0 center, +1 right

    public float DeviationSensitivity = 1f;

    float m_posChangeSpeed;
    float m_posChangeOffset;

    public float PosMinChangeSpeed = 6f;

    public float PosMaxChangeSpeed = 8f;

    public float LookForwardValue = 5f;

    public bool RunAroundCars = true;

    public float DangerousRadius = 16f;

    public float PresserRadius = 20f;

    public float AvoidRotationFactor = 0.5f;

    public float PresserRotationFactor = 0.35f;

    public float AvoidDumpingFactor = 1.5f;

    public float AvoidClamps = 0.5f;

    private Movenment[] m_movenments;

    private void Awake()
    {
        Movenment = GetComponentInParent<Movenment>();
        Shockable = GetComponentInParent<ShockableCar>();

        m_posChangeSpeed = Random.Range(PosMinChangeSpeed, PosMaxChangeSpeed) * (Random.value > 0.5f ? -1f : +1f);
        m_posChangeOffset = PositionAtTheRoad;

        m_movenments = FindObjectsOfType<Movenment>();
    }

    private Movenment.TrackingData FetchTrackingFromFuture()
    {
        return Movenment.FetchTrackingData(transform.up * LookForwardValue);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        var tracking = FetchTrackingFromFuture();
        Vector2 targetDirection = CalculateTargetDirection(tracking);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position - Vector3.forward, tracking.RoadDirection * DirectionScale * 5f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position - Vector3.forward, tracking.DirectionToRoadCenter * 5f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position - Vector3.forward, targetDirection * 5f);
    }



    private void FixedUpdate()
    {
        PositionAtTheRoad = Mathf.Sin(Time.time / m_posChangeSpeed * Mathf.PI + Mathf.PI * m_posChangeOffset);

        var tracking = FetchTrackingFromFuture();
        Vector2 targetDirection = CalculateTargetDirection(tracking);

        Vector2 currentDirection = transform.up;
        float deviation = targetDirection.x * currentDirection.y - targetDirection.y * currentDirection.x;

        Movenment.EngineIsTurnedOn = true;
        Movenment.RotationDirection = deviation * DeviationSensitivity;


        if (RunAroundCars)
        {
            float clampMin = -1f, clampMax = 1f;

            if ((tracking.CarPosition - tracking.RoadCenter).magnitude > tracking.RoadRadius - SafeBoards)
            {
                if (Vector3.Cross(tracking.DirectionToRoadCenter, tracking.RoadDirection).z < 0f)
                    clampMax = -AvoidClamps; // To run out from bounds.
                else 
                    clampMin = +AvoidClamps;
            }

            float sign = -1, radius = DangerousRadius;

            if (Shockable.IgnoreCollisionShocks > 0)
            {
                sign = +1f; radius = PresserRadius;
            }

            radius *= radius;

            bool anyFound = false;
            Vector2 directionToGo = default;
            foreach (var otherMovenment in m_movenments)
            {
                if (otherMovenment == Movenment)
                    continue;

                Vector2 delta =
                otherMovenment.Rigidbody2D.position -
                Movenment.Rigidbody2D.position;

                if (delta.sqrMagnitude < radius)
                {
                    directionToGo += sign * delta;
                    anyFound = true;
                }
            }

            if (anyFound)
            {
                directionToGo.Normalize();
                Movenment.RotationDirection *= AvoidDumpingFactor;
                Movenment.RotationDirection += Mathf.Clamp(Vector2.Dot(transform.right, directionToGo), clampMin, clampMax) * AvoidRotationFactor;
                Movenment.RotationDirection = Mathf.Clamp(Movenment.RotationDirection, -1, +1);
                return;
            }

        }
    }

    private Vector2 CalculateTargetDirection(Movenment.TrackingData tracking)
    {
        Vector2 offset = (Vector2)Vector3.Cross(
                                    -Vector3.forward,
                                    tracking.RoadDirection * DirectionScale
                                    ) * PositionAtTheRoad * (tracking.RoadRadius - SafeBoards);
        return Vector2.Lerp(
                tracking.RoadDirection * DirectionScale,
                ((tracking.RoadCenter + offset) - tracking.CarPosition).normalized,
                Mathf.Min(
                    (
                        tracking.CarPosition -
                        (
                            tracking.RoadCenter + offset

                        )
                    ).magnitude / (tracking.RoadRadius - SafeBoards),
                    maxLerpFactor
                )
            ).normalized;
    }
}
