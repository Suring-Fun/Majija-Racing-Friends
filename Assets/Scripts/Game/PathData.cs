
using System;
using System.Linq;
using UnityEngine;

public class PathData : MonoBehaviour
{
    [field: SerializeField]
    public RoadSystem RoadSystem { get; private set; }

    [field: SerializeField]
    public float Step { get; private set; } = 20f;

    [field: SerializeField]
    public float DistanceToWater { get; private set; } = 12f;

    [field: SerializeField]
    public float DistanceToDeepWater { get; private set; } = 4f;

    [field: SerializeField]
    public bool EdgesAreSolid { get; private set; } = false;

    private (Vector3, float)[] Points { get; set; }

    public float TotalLength { get; private set; }

    private void Awake()
    {
        Points = RoadSystem.GetPoints(Step).ToArray();
        TotalLength = GetTotalLength();
        Debug.Log(Points.Length);
    }

    public (float nearestDistance, float projection) DistacneBetweenLineAndPoint(Vector2 line1, Vector2 line2, Vector2 point)
    {
        float projection = Vector2.Dot(line2 - line1, point - line1) / (line2 - line1).magnitude;

        if (Vector2.Dot(line2 - line1, point - line1) < 0f)
        {
            return ((point - line1).magnitude, projection);
        }
        else if (Vector2.Dot(line1 - line2, point - line2) < 0f)
        {
            return ((point - line2).magnitude, projection);
        }
        else
        {
            Vector2 lhs = Vector3.Cross(Vector3.forward, line2 - line1);
            return (Mathf.Abs(Vector2.Dot(lhs, point - line1) / (lhs).magnitude), projection);
        }
    }

    public Vector2 SnapPointToLine(Vector2 line1, Vector2 line2, Vector2 point)
    {
        if (Vector2.Dot(line2 - line1, point - line1) < 0f)
        {
            return line1;
        }
        else if (Vector2.Dot(line1 - line2, point - line2) < 0f)
        {
            return line2;
        }
        else
        {
            Vector2 lhs = line2 - line1;
            float lhsLen = (lhs).magnitude;
            return line1 + Vector2.Dot(lhs, point - line1) * lhs / (lhsLen * lhsLen);
        }
    }

    private float GetTotalLength()
    {
        float t = 0f;
        for (int x = 0; x < Points.Length; ++x)
        {
            (Vector2 c, float cr) = Points[x];
            (Vector2 n, float nr) = Points[(x + 1) % Points.Length];
            t += (n - c).magnitude;
        }
        return t;
    }

    public void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {

            for (int x = 0; x < Points.Length; ++x)
            {
                (Vector2 c, float cr) = Points[x];
                (Vector2 n, float nr) = Points[(x + 1) % Points.Length];

                Gizmos.color = Color.green;
                Gizmos.DrawLine((Vector3)c - Vector3.forward, (Vector3)n - Vector3.forward);
            }
        }
    }

    public (Vector2 point, float radius) GetNearestPointFast(Vector2 point, int skipStep = 20)
    {

        Vector2 p = default;
        float r = 0f;

        float min = float.PositiveInfinity;

        for (int x = 0; x < Points.Length; x += skipStep)
        {
            (Vector2 c, float cr) = Points[x];

            float d = (c - point).sqrMagnitude;
            if (d < min)
            {
                min = d;
                p = c;
                r = cr;
            }
        }

        return (p, r);
    }

    public (float distance, float position, float radius, Vector2 roadDirection) GetLocationAtTrack(Vector2 currentPoint)
    {
        float minDistance = float.PositiveInfinity;
        float distancePassed = 0f;

        float selectedDistancePassed = 0f;
        float selectedDistanceFromTrack = float.PositiveInfinity;
        float selectedRadius = 0f;
        float sign = 0f;

        Vector2 selectedRoadDirection = default;

        for (int x = 0; x < Points.Length; ++x)
        {
            (Vector2 c, float cr) = Points[x];
            (Vector2 n, float nr) = Points[(x + 1) % Points.Length];

            float snippedLenght = (n - c).magnitude;

            (float distance, float projection) = DistacneBetweenLineAndPoint(c, n, currentPoint);

            if (distance < minDistance)
            {
                minDistance = distance;
                sign = Vector2.Dot(Vector3.Cross(-Vector3.forward, n - c), currentPoint - n);
                selectedDistancePassed = distancePassed + projection;
                selectedDistanceFromTrack = distance;
                selectedRadius = Mathf.Lerp(cr, nr, projection / snippedLenght);
                selectedRoadDirection = n - c;
            }

            distancePassed += snippedLenght;
        }

        return (selectedDistanceFromTrack * Mathf.Sign(sign), selectedDistancePassed, selectedRadius, selectedRoadDirection.normalized);
    }

    public (Vector2 point, Vector2 direction, float radius) GetNearestPoint(Vector2 currentPoint)
    {
        Vector2 l1 = default, l2 = default;
        float r1 = 0f, r2 = 0f;

        Vector2 sp = default;

        float min = float.PositiveInfinity;

        for (int x = 0; x < Points.Length; ++x)
        {
            (Vector2 c, float cr) = Points[x];
            (Vector2 n, float nr) = Points[(x + 1) % Points.Length];

            Vector2 snappedP = SnapPointToLine(c, n, currentPoint);

            float d = (snappedP - currentPoint).sqrMagnitude;
            if (min > d)
            {
                min = d;

                l1 = c;
                l2 = n;

                r1 = cr;
                r2 = nr;

                sp = snappedP;
            }
        }

        return (
            sp,
            (l2 - l1).normalized,
            Mathf.Lerp(r1, r2, (sp - l1).magnitude / (l2 - l1).magnitude)
        );
    }

    public (Vector2 point, Vector2 dir) GetStartLineInfo()
    {
        return (Points[0].Item1, (Points[1].Item1 - Points[0].Item1).normalized);
    }
}
