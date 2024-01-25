using UnityEngine;

public class AIMovenmentController : MonoBehaviour
{
    public Movenment Movenment { get; private set; }

    public float SafeBoards = 2f;

    public float maxLerpFactor = 0.8f;

    [field: Tooltip("1 left, 0 center, +1 right")]
    [field: Range(-1f, 1f)]
    public float PositionAtTheRoad = 0f; // -1 left, 0 center, +1 right

    public float DeviationSensitivity = 1f;

    float m_posChangeSpeed;
    float m_posChangeOffset;

    public float PosMinChangeSpeed = 6f;
    
    public float PosMaxChangeSpeed = 8f;

    public float LookForwardValue = 5f;

    private void Awake()
    {
        Movenment = GetComponent<Movenment>();
        m_posChangeSpeed = Random.Range(PosMinChangeSpeed, PosMaxChangeSpeed) * (Random.value > 0.5f ? -1f: +1f);
        m_posChangeOffset = PositionAtTheRoad;
    }

    private Movenment.TrackingData FetchTrackingFromFuture() {
        return Movenment.FetchTrackingData(transform.up * LookForwardValue);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        var tracking = FetchTrackingFromFuture();
        Vector2 targetDirection = CalculateTargetDirection(tracking);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position - Vector3.forward, tracking.RoadDirection * 5f);

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
    }

    private Vector2 CalculateTargetDirection(Movenment.TrackingData tracking)
    {
        Vector2 offset = (Vector2)Vector3.Cross(
                                    -Vector3.forward,
                                    tracking.RoadDirection
                                    ) * PositionAtTheRoad * (tracking.RoadRadius - SafeBoards);
        return Vector2.Lerp(
                tracking.RoadDirection,
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
