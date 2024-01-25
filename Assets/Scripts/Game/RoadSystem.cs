using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadSystem : MonoBehaviour {

    public struct Point {
        public Transform Center;

        public float Radius => Center.GetChild(1).localPosition.magnitude;

        public Point(Transform c) => Center = c;
    }

    public struct Spline {
        public Transform Start;
        public Transform Finish;

        public Vector3 CalculatePosition(float norm01) {
            return Vector3.Lerp(
                    Vector3.Lerp(
                        Vector3.Lerp(Start.position, Start.GetChild(0).position, norm01),
                        Vector3.Lerp(Start.GetChild(0).position, 2f * Finish.position - Finish.GetChild(0).position, norm01),
                        norm01
                        ),
                    Vector3.Lerp(
                        Vector3.Lerp(Start.GetChild(0).position, 2f * Finish.position - Finish.GetChild(0).position, norm01),
                        Vector3.Lerp(2f * Finish.position - Finish.GetChild(0).position, Finish.position, norm01),
                        norm01
                        ),
                    norm01
                    );
        }

        public float CalcRoughLength(float step = 0.1f) {
            float len = 0f;

            for (float x = 0f; x < 1f; x += step) {
                len += (CalculatePosition(x) - CalculatePosition(x + step)).magnitude;
            }

            return len;
        }
    }

    public float GizmoRoughty = 10f;

    public IEnumerable<(Vector3 point, float distance)> GetPoints(float step = 0.5f) {
        int chount = transform.childCount;

        for (int x = 0; x < chount; ++x) {
            var ch1 = transform.GetChild(x);
            var ch2 = transform.GetChild((x + 1) % chount);

            Spline s = new Spline() {
                Start = ch1,
                Finish = ch2
            };
            Point p1 = new(ch1);
            Point p2 = new(ch2);

            float r1 = p1.Radius;
            float r2 = p2.Radius;

            float len = s.CalcRoughLength();


            float step01 = step / len;

            for (float y = 0f; y < 1f; y += step01)
                yield return (s.CalculatePosition(y), Mathf.Lerp(r1, r2, y));
        }
    }

    public Material material;

    public IEnumerable<(Vector3 left, Vector3 right)> GetRoadSides(float step) {
        if (step < 0.1f)
            yield break;

        var points = GetPoints(step).ToArray();


        for (int x = 0; x < points.Length; ++x) {
            var pc = points[x];
            var pp = points[x == 0 ? points.Length - 1 : x - 1];
            var pn = points[(x + 1) % points.Length];

            float lenSum = (pn.point - pc.point).magnitude + (pc.point - pp.point).magnitude;
            float len1 = (pn.point - pc.point).magnitude / lenSum;
            float len2 = (pc.point - pp.point).magnitude / lenSum;

            Vector3 dirInTheAngle =
                (
                Vector3.Cross(Vector3.forward, pn.point - pc.point).normalized * len2 +
                Vector3.Cross(Vector3.forward, pc.point - pp.point).normalized * len1
                ).normalized;

            // Mult to this to Mathf.Sign(Vector3.Dot(dirInTheAngle, pc.point - pp.point)) to get deffered by sign.
            dirInTheAngle *= pc.distance;

            Vector2 pointLeft = pc.point + dirInTheAngle;
            Vector2 pointRight = pc.point - dirInTheAngle;

            yield return (pointLeft, pointRight);
        }
    }


    public void OnDrawGizmos() {


        var points = GetRoadSides(GizmoRoughty).ToArray();


        for (int x = 0; x < points.Length; ++x) {

            var pc = points[x];
            var pn = points[(x + 1) % points.Length];


            Gizmos.color = Color.red;
            Gizmos.DrawLine(pc.left, pc.right);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pc.left, pn.left);
            Gizmos.DrawLine(pc.right, pn.right);
        }
    }
}
