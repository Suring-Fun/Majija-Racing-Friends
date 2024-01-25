using UnityEngine;


public class PlayerTouchMovenmentController : MonoBehaviour
{
    Movenment m;

    public RectTransform Stick;

    public float StickRadius = 1f;

    private StickFetch m_movenmentStick;

    void Awake()
    {
        m = GetComponent<Movenment>();
        m_movenmentStick = Stick.gameObject.AddComponent<StickFetch>();
        m_movenmentStick.Radius = StickRadius;
        m_movenmentStick.Mult = Vector2.right;

    }

    // Update is called once per frame
    void Update()
    {
        m.EngineIsTurnedOn = m_movenmentStick.IsInUse;
        if (m.EngineIsTurnedOn)
        {
            m.RotationDirection = m_movenmentStick.Position.x;
        }
    }
}
