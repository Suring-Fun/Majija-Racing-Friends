using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SpawnCollisionResolver : MonoBehaviour
{
    private RescueableCar[] m_allRescuable;

    private Movenment[] m_allMovenments;

    private Movenment.TrackingData[] m_trackingDataBuffer;

    public float SafeRadius = 15f;

    public float Step = 0.5f;

    public int MaxRepeats = 50;

    void Start()
    {
        m_allRescuable = FindObjectsOfType<RescueableCar>();
        m_allMovenments = m_allRescuable.Select(x => x.GetComponent<Movenment>()).ToArray();
        m_trackingDataBuffer = new Movenment.TrackingData[m_allRescuable.Length];
    }

    void UpdateBuffer()
    {
        for (int x = 0; x < m_allRescuable.Length; ++x)
        {
            m_trackingDataBuffer[x] = m_allRescuable[x].DataToRescueWith;
        }
    }

    void DoStepAcrossRoad(int car, float sign)
    {
        var dataToRescueWith = m_allRescuable[car].DataToRescueWith;
        dataToRescueWith.RoadCenter += dataToRescueWith.RoadDirection * Step * sign;
        m_allRescuable[car].DataToRescueWith = m_allMovenments[car].FetchTrackingDataForGlobalPoint(dataToRescueWith.RoadCenter);
    }

    bool DoResolveStepForPair(int carIndex1, int carIndex2)
    {
        var rescuable1 = m_allRescuable[carIndex1];
        var rescuable2 = m_allRescuable[carIndex2];

        if (rescuable1.IsDataToRescueWithLocked && rescuable2.IsDataToRescueWithLocked)
            return false; // We can't resolve anything.

        var buffered1 = m_trackingDataBuffer[carIndex1];
        var buffered2 = m_trackingDataBuffer[carIndex2];

        Vector2 direction = buffered2.RoadCenter - buffered1.RoadCenter;
        if (direction.sqrMagnitude < SafeRadius * SafeRadius)
        {
            float sign = Mathf.Sign(Vector2.Dot(direction, buffered1.RoadDirection));

            if (!rescuable1.IsDataToRescueWithLocked)
                DoStepAcrossRoad(carIndex1, -sign);

            if (!rescuable2.IsDataToRescueWithLocked)
                DoStepAcrossRoad(carIndex2, +sign);

            return true;
        }

        return false;
    }

    void LateUpdate()
    {
        bool isResolved;
        int repeats = 0;

        do
        {
            UpdateBuffer();
            isResolved = true;

            for (int i1 = 0; i1 < m_allRescuable.Length - 1; ++i1)
            {
                for (int i2 = i1 + 1; i2 < m_allRescuable.Length; ++i2)
                {
                    isResolved &= !DoResolveStepForPair(i1, i2);
                }
            }

            ++repeats;
        }
        while (!isResolved && repeats < MaxRepeats);
    }
}
