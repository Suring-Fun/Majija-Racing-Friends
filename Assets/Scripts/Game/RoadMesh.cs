using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadMesh : MonoBehaviour
{
    private RoadSystem system;

    public float Roughty = 20f;

    public Material material;

    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        system = GetComponent<RoadSystem>();


        var points = system.GetRoadSides(Roughty).ToArray();

        mesh = new Mesh();
        List<Vector3> vertices = new();
        List<int> triangles = new();
        List<Vector2> uv = new();

        int counter = 0;

        float uvPos = 0f;

        for (int x = 0; x < points.Length; ++x)
        {

            var pc = points[x];
            var pn = points[(x + 1) % points.Length];

            vertices.Add(pc.left);
            vertices.Add(pc.right);
            vertices.Add(pn.right);
            vertices.Add(pn.left);

            triangles.Add(counter + 2);
            triangles.Add(counter + 1);
            triangles.Add(counter + 0);

            triangles.Add(counter + 0);
            triangles.Add(counter + 3);
            triangles.Add(counter + 2);


            counter += 4;

            float distanceLeft =
                Mathf.Min(
                    (pc.left - pn.left).magnitude,
                    (pc.right - pn.right).magnitude
                    ) * 0.25f +
                Mathf.Max(
                (pc.left - pn.left).magnitude,
                (pc.right - pn.right).magnitude
                ) * 0.75f;

            uv.Add(new Vector2(0f, uvPos));
            uv.Add(new Vector2(1f, uvPos));

            uvPos += distanceLeft;

            uv.Add(new Vector2(1f, uvPos));
            uv.Add(new Vector2(0f, uvPos));
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
    }

    private void Update()
    {
        Graphics.RenderMesh(
            new RenderParams(material),
            mesh,
            0,
            transform.localToWorldMatrix
            );
    }
}
