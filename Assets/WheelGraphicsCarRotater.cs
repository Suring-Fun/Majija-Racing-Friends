using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelGraphicsCarRotater : MonoBehaviour
{
    private Movenment m_movenment;

    public float MaxRotation = 30;

    public Vector2 Offset;

    public float NormalizedAngularSpeed = 1.5f; 

    private Vector3 m_oldPos;

    // Start is called before the first frame update
    void Start()
    {
        m_movenment = GetComponentInParent<Movenment>();
        m_oldPos = transform.localPosition;
    }

    private float rotationDirection;

    // Update is called once per frame
    void LateUpdate()
    {
        float step = Time.deltaTime * NormalizedAngularSpeed;
        rotationDirection += Mathf.Clamp(
            m_movenment.RotationDirection-rotationDirection,
            -step,
            +step
        );

        transform.localEulerAngles = Vector3.forward * MaxRotation * rotationDirection;
        transform.localPosition = new Vector3(
            m_oldPos.x + Offset.x * rotationDirection,
            m_oldPos.y + Offset.y * Mathf.Abs(rotationDirection),
            m_oldPos.z
        );
    }
}
