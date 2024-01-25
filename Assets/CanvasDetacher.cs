using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDetacher : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(null, false);
    }
}
