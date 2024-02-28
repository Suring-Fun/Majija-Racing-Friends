using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RescueableCar : MonoBehaviour
{
    private Movenment m_movenment;
    private RoadPositionTracker m_tracker;
    private Vector2 m_swimDirection;

    [field: SerializeField]
    public float SwimSpeed = 5f;

    [field: SerializeField]
    public float SwimAcceleration = 15f;

    public float GoToWaterStartCof = 0.1f;

    public float RescuePause = 1f;

    public float CameraMoveTime = 1f;

    public float Min180DotResult = 0.5f;

    public bool ReveseAngle = false;

    private bool m_isSwimming;
    private bool m_rescuing;

    private Camera m_cum;
    private Vector3 m_cumPos;
    private Quaternion m_cumVRot;

    private Movenment.TrackingData m_dataToRescureWith;

    public bool IsResquing => m_rescuing;
    public bool IsSwimming => m_isSwimming;

    private void Awake()
    {
        m_movenment = GetComponent<Movenment>();
        m_tracker = GetComponent<RoadPositionTracker>();
        m_cum = GetComponentInChildren<Camera>();
        if (m_cum)
        {
            m_cumPos = m_cum.transform.localPosition;
            m_cumVRot = m_cum.transform.localRotation;
        }
    }

    public void RunRescueProgram(Movenment.TrackingData trackingData)
    {
        if (m_rescuing || m_isSwimming)
            return;
        m_rescuing = true;
        m_dataToRescureWith = trackingData;
        StartCoroutine(CumRescueProgram());
    }

    private IEnumerator CumRescueProgram()
    {
        if (m_tracker)
            m_tracker.enabled = false;
        m_movenment.FreeFly++;

        yield return new WaitForSeconds(RescuePause);

        Vector2 roadDirection = m_dataToRescureWith.RoadDirection;
        if (ReveseAngle)
            roadDirection = -roadDirection;

        float targetRot = Mathf.Atan2(-roadDirection.x, roadDirection.y) * Mathf.Rad2Deg;

        if (m_cum)
        {
            float time = 0f;

            Vector2 oldPos = m_cum.transform.position;
            float oldRot = m_cum.transform.eulerAngles.z;

            Vector2 targetPos = m_dataToRescureWith.RoadCenter + roadDirection * m_cumPos.y;

            while (time < CameraMoveTime)
            {
                float _01 = time / CameraMoveTime;
                Vector3 currPos = (Vector3)Vector2.Lerp(
                    oldPos,
                    targetPos,
                    _01
                ) + m_cumPos.z * Vector3.forward;

                float angle = Mathf.LerpAngle(oldRot, targetRot, _01);

                m_cum.transform.position = currPos;
                m_cum.transform.eulerAngles = Vector3.forward * angle;

                time += Time.deltaTime;
                yield return null;
            }
        }
        else
            yield return new WaitForSeconds(CameraMoveTime);

        var body = m_movenment.Rigidbody2D;
        body.position = m_dataToRescureWith.RoadCenter;
        body.rotation = targetRot;

        GetComponent<ShockableCar>().AbortShocking();
        m_movenment.ResetAllSpeeds();
        body.velocity = default;


        if (m_tracker)
            m_tracker.enabled = true;
        m_movenment.FreeFly--;

        m_isSwimming = false;
        m_rescuing = false;


        if (m_cum)
        {
            yield return new WaitForFixedUpdate();
            m_cum.transform.parent = transform;
            m_cum.transform.localPosition = m_cumPos;
            m_cum.transform.localRotation = m_cumVRot;
        }
    }

    private void FixedUpdate()
    {
        var tracking = m_movenment.Tracking;
        bool itWasAtTheGround = !m_isSwimming;

        if (m_isSwimming |= (tracking.CarInTheWater && !tracking.EdgeIsSolid))
        {
            m_swimDirection = -tracking.DirectionToRoadCenter;
            var body = m_movenment.Rigidbody2D;

            if (itWasAtTheGround)
            {
                m_dataToRescureWith = tracking;
                body.velocity *= GoToWaterStartCof;
                StartCoroutine(CumRescueProgram());

                if (m_cum)
                {
                    m_cum.transform.parent = transform.parent;
                }

            }

            float speedInTheDirection = Vector2.Dot(m_swimDirection, body.velocity);

            float step = Mathf.Clamp(SwimAcceleration - speedInTheDirection, 0f, Time.deltaTime * SwimAcceleration);

            body.velocity += step * m_swimDirection;
        }
    }

    internal void Run180RescueProgramIfRequired()
    {
        var td = m_movenment.FetchTrackingData();
        if (Vector2.Dot(transform.up, ReveseAngle ? -td.RoadDirection : td.RoadDirection) < Min180DotResult)
            RunRescueProgram(td);
    }
}
