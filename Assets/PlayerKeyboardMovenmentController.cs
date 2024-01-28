using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardMovenmentController : MonoBehaviour
{
    Movenment m;


    void Awake()
    {
        m = GetComponentInParent<Movenment>();
    }

    void Update()
    {
        bool leftEnabled = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool rightEnabled = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool engineEnabled = (leftEnabled | rightEnabled) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        float rotationDirection = 0f;

        if (leftEnabled)
            rotationDirection -= 1f;

        if (rightEnabled)
            rotationDirection += 1f;

        m.EngineIsTurnedOn = engineEnabled;
        m.RotationDirection = rotationDirection;
    }
}
