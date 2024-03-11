using UnityEngine;


public class PlayerTouchMovenmentController : MonoBehaviour
{
    Movenment m;

    public RectTransform Stick;

    public RectTransform Ruddle;

    public float RuddleMoveSize = 3.5f / 2f;

    public float StickRadius = 1f;

    public AnimationCurve Factor;

    private StickFetch m_movenmentStick;

    void Awake()
    {
        m = GetComponentInParent<Movenment>();
        m_movenmentStick = Stick.gameObject.AddComponent<StickFetch>();
        m_movenmentStick.Radius = StickRadius;
        m_movenmentStick.Mult = Vector2.right;

    }

    // Update is called once per frame
    void Update()
    {
        Ruddle.localPosition = Vector3.right * m_movenmentStick.Position.x;

        m.EngineIsTurnedOn = m_movenmentStick.IsInUse;
        if (m.EngineIsTurnedOn)
        {
            m.RotationDirection = Factor.Evaluate(Mathf.Abs(m_movenmentStick.Position.x)) * Mathf.Sign(m_movenmentStick.Position.x);
        }
    }
}
