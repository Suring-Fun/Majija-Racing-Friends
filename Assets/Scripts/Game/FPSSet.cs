using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// That's important to set large fps for this kind of dynamic games.
public class FPSSet : MonoBehaviour
{
    [field: SerializeField]
    public int FPS { get; private set; } = 60;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = FPS;
    }

}
