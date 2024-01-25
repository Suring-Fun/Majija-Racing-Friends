using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Movenment : MonoBehaviour
{
    public struct TrackingData
    {
        public Vector2 RoadCenter, RoadDirection, DirectionToRoadCenter;
        public float RoadRadius;

        public Vector2 CarPosition;

        public bool CarIsAtTheRoad;

        public bool CarInTheWater;
    }

    private float m_speed = 0f;

    private float m_angularSpeed = 0f;

    [field: SerializeField]
    public float MaxSpeed { get; private set; } = 30f;

    [field: SerializeField]
    public float Acceleration { get; private set; } = 50f;

    [field: SerializeField]
    public float BackwardAcceleration { get; private set; } = 30f;

    [field: SerializeField]
    public float AngularAcceleration { get; private set; } = 120f;

    [field: SerializeField]
    public float MaxAngularSpeed { get; private set; } = 90f;

    [field: SerializeField]
    public float FreeSwimSpeed { get; private set; } = 10f;


    public Rigidbody2D Rigidbody2D { get; private set; }

    public PathData PathData { get; private set; }

    public bool EngineIsTurnedOn { get; set; }

    public int FreeFly { get; set; }

    public float OutOfRoadScaleFactor { get; private set; } = 0.6f;

    public TrackingData Tracking =>
        m_tracking.HasValue ?
        m_tracking.Value :
        (m_tracking = FetchTrackingData()).Value;

    public TrackingData FetchTrackingData(Vector2 offset = default)
    {
        Vector2 position = Rigidbody2D.position + offset;
        (var pos, var dir, var rad) = PathData.GetNearestPoint(position);
        return new TrackingData()
        {
            CarPosition = position,
            RoadCenter = pos,
            RoadDirection = dir,
            DirectionToRoadCenter = (pos - position).normalized,
            RoadRadius = rad,
            CarIsAtTheRoad = (pos - position).magnitude <= rad,
            CarInTheWater = (pos - position).magnitude > rad + PathData.DistanceToWater + PathData.DistanceToDeepWater
        };
    }

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        //Input.multiTouchEnabled = false;
        Input.simulateMouseWithTouches = true;
        PathData = FindObjectOfType<PathData>();
    }

    float m_targetAngularSpeed = 0f;

    private float m_roationDirection = 0f;
    private TrackingData? m_tracking;

    public float RotationDirection
    {
        get => m_roationDirection;
        set => m_roationDirection = Mathf.Clamp(value, -1f, +1f);
    }
    public float CurrentSpeed => m_speed;

    public void ResetAllSpeeds()
    {
        m_speed = 0f;
        m_angularSpeed = 0f;
        m_tracking = null;
    }

    void FixedUpdate()
    {
        //Tracking = FetchTrackingData();
        m_tracking = null;

        if (FreeFly > 0)
            return;

        float currentAcceleration = -BackwardAcceleration;

        if (EngineIsTurnedOn)
        {
            m_targetAngularSpeed = RotationDirection;
            m_targetAngularSpeed *= -MaxAngularSpeed;
            currentAcceleration = Acceleration;
        }

        m_speed += currentAcceleration * Time.fixedDeltaTime;
        m_speed = Mathf.Clamp(m_speed, 0f, MaxSpeed);

        float angularSpeedDelta = m_targetAngularSpeed - m_angularSpeed;
        float accelerationPerFrame = AngularAcceleration * Time.fixedDeltaTime;


        angularSpeedDelta = Mathf.Clamp(angularSpeedDelta, -accelerationPerFrame, +accelerationPerFrame);
        m_angularSpeed += angularSpeedDelta;
        //m_angularSpeed = targetAngularSpeed;

        float scale = Tracking.CarIsAtTheRoad ? 1f : OutOfRoadScaleFactor;

        Rigidbody2D.rotation += m_angularSpeed * Time.fixedDeltaTime * (m_speed / MaxSpeed) * scale;

        Rigidbody2D.velocity = (Vector2)(transform.up * m_speed * scale);
    }

    public Action MultSpeed(float moveMultFactor, float rotateMultFactor)
    {
        float moveSpeed = MaxSpeed;
        float angularSpeed = MaxAngularSpeed;

        MaxSpeed *= moveMultFactor;
        MaxAngularSpeed *= rotateMultFactor;

        m_speed *= moveMultFactor;
        m_angularSpeed *= rotateMultFactor;

         return () => { 
            MaxSpeed = moveSpeed;
            MaxAngularSpeed = angularSpeed;

            m_speed /= moveMultFactor;
            m_angularSpeed /= rotateMultFactor;
         };
    }
}
