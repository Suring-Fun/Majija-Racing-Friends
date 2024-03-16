using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPositionTracker : MonoBehaviour
{
    public int Lap { get; private set; }

    public float DecimalPart { get; private set; } = 1f;

    public float DecimalLap => Lap + DecimalPart;

    private PathData pathData;

    public (int my, int total) CalculatePlace()
    {
        var all = FindObjectsOfType<RoadPositionTracker>();
        Array.Sort<RoadPositionTracker>(
        all,
        (a, b) =>
            (b.DecimalLap - a.DecimalLap) switch
            {
                > 0 => 1,
                < 0 => -1,
                _ => 0
            }
        );

        for (int x = 0; x < all.Length; ++x)
        {
            if (all[x] == this)
                return (x, all.Length);
        }

        return (-1, all.Length);
    }

    void Awake()
    {
        pathData = FindObjectOfType<PathData>();
    }

    void FixedUpdate()
    {
        var pos = pathData.GetLocationAtTrack(transform.position);
        pos.position /= pathData.TotalLength;

        float delta = pos.position - DecimalPart;
        if (delta < -0.5f)
        {
            Lap++;
        }
        else if (delta > +0.5f)
        {
            Lap--;
        }

        DecimalPart = pos.position;
    }
}
