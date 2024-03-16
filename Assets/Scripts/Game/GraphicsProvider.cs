using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphicsProvider : MonoBehaviour
{
    public string KindOfGraphics;

    public GameObject[] Graphics;

    public static GraphicsProvider GetProviderFromScene(string kind) 
    => FindObjectsOfType<GraphicsProvider>().
        Where(x => x.KindOfGraphics == kind).
        First();

    public GameObject SelectRandomGraphics()
        => Graphics[Random.Range(0, Graphics.Length)];
}
