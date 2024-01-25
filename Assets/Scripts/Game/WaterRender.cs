using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRender : MonoBehaviour
{
    [field: SerializeField]
    public PathData PathData { get; private set; }

    public float PixelsPerUnit = 4;


    public int SkipValue = 10;


    public Vector2 MaxUnits = new Vector2Int(512, 512);

    public Vector2 Offset = new Vector2Int(-256, -256);

    [field: SerializeField]
    public Color Color { get; private set; } = Color.blue;

    public float texCoof = 0.01f;


    public string MaterialPath;

    private Mesh mesh;

    private Material material;

    void Start()
    {
        var sizes = Vector2Int.FloorToInt(PixelsPerUnit * MaxUnits);
        Texture2D t = new Texture2D(sizes.x, sizes.y);

        t.filterMode = FilterMode.Bilinear;

        for (int y = 0; y < sizes.y; ++y)
        {
            for (int x = 0; x < sizes.x; ++x)
            {
                Vector2 p = new Vector2(x + .5f, y + .5f) / PixelsPerUnit + Offset;
                (Vector2 roadP, _, float radius) = PathData.GetNearestPoint(p);
                t.SetPixel(
                    x, 
                    y, 
                    new Color(
                        (Color.r), 
                        (Color.g), 
                        (Color.b), 
                        Mathf.Clamp(
                            ((p - roadP).magnitude - PathData.DistanceToWater - radius) * texCoof,
                            -1f, 1f
                            ) * 0.5f + 0.5f)
                            );
            }
        }

        t.Apply();

        mesh = new Mesh();
        mesh.vertices = new Vector3[] {
            new Vector3(Offset.x, Offset.y),
            new Vector3(Offset.x + MaxUnits.x, Offset.y),
            new Vector3(Offset.x + MaxUnits.x, Offset.y + MaxUnits.y),
            new Vector3(Offset.x, Offset.y + MaxUnits.y)
        };
        mesh.uv = new Vector2[] {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f)
        };

        mesh.triangles = new int[] {
            2, 1, 0,
            3, 2, 0
        };

        material = new Material(Shader.Find(MaterialPath));
        material.mainTexture = t;
    }

    void Update()
    {
        Graphics.RenderMesh(new RenderParams(material), mesh, 0, transform.localToWorldMatrix);
    }
}
