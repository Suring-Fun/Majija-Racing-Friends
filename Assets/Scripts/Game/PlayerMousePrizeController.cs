using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMousePrizeController : MonoBehaviour
{
    Transform m_car;

    PrizeHost m_prizeHost;

    Camera m_camera;

    EventSystem m_eventSystem;

    PointerEventData m_eventData;

    List<RaycastResult> m_raycastResults;

    private void Awake()
    {
        m_prizeHost = GetComponentInParent<PrizeHost>();
        m_car = m_prizeHost.transform;

        m_camera = m_prizeHost.GetComponentInChildren<Camera>();

        m_eventSystem = FindObjectOfType<EventSystem>();
        m_eventData = new PointerEventData(m_eventSystem);

        m_raycastResults = new();
    }

    private Vector2 m_lastSavedDirection;

    private bool m_actionPlanned;

    [field: SerializeField]
    public float MinDistance = 2f;

    public Vector2 DirectionOfCursor { get; private set; }

    public bool CursorDistanceIsShort { get; private set; }

    public bool PreviewIsInfinity => m_prizeHost.PreviewIsInfinity;

    private bool m_mousePressed;
    private bool m_mousePressedOldState;

    private void Update()
    {
        Vector3 worldDir = m_camera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward) - m_car.position;
        Vector2 direction = new Vector2(
            Vector2.Dot(transform.right, worldDir),
            Vector2.Dot(transform.up, worldDir)
        );

        bool distanceIsShort = direction.sqrMagnitude < MinDistance * MinDistance;

        DirectionOfCursor = direction;
        CursorDistanceIsShort = distanceIsShort;

        if (Input.GetMouseButtonDown(0))
        {
            if (!CheckIfCursorHasObstacles())
                m_mousePressed = true;
        }
        else if (Input.GetMouseButtonUp(0))
            m_mousePressed = false;

        if (m_mousePressed && m_prizeHost.MainPrizeAccessed)
        {
            m_prizeHost.MainPrizeIsLocked = true;
            m_actionPlanned = true;

            if (distanceIsShort)
            {
                m_prizeHost.DisableApplyView();
                m_actionPlanned = false;
                goto Finish;
            }

            if (m_prizeHost.MainPrize.ApplyMode == IPrize.PrizeApplyMode.JustApply)
            {
                m_lastSavedDirection = Vector2.up;
                m_prizeHost.EnableApplyPreview(Vector2.up);
                goto Finish;
            }

            Vector2 directionNormalized = direction.normalized;

            float _01 = m_prizeHost.MainPrize.PreviewDistance(directionNormalized);
            directionNormalized = m_prizeHost.MainPrize.PreviewDirection(directionNormalized);
            if (float.IsFinite(_01))
            {
                directionNormalized *= Mathf.Clamp01(direction.magnitude / _01);
            }

            m_prizeHost.EnableApplyPreview(directionNormalized);
            m_lastSavedDirection = directionNormalized;
        }
        else
        {

            if (m_actionPlanned && m_prizeHost.MainPrizeAccessed && m_mousePressedOldState)
            {
                m_prizeHost.ApplyPrize(m_lastSavedDirection);
            }

            m_prizeHost.DisableApplyView();
            m_prizeHost.MainPrizeIsLocked = false;
        }

    Finish:
        m_mousePressedOldState = m_mousePressed;
    }

    bool CheckIfCursorHasObstacles()
    {
        m_eventData.position = Input.mousePosition;
        m_raycastResults.Clear();
        m_eventSystem.RaycastAll(m_eventData, m_raycastResults);

        return m_raycastResults.Count > 0;
    }
}
