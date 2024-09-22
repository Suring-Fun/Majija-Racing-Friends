using System;
using UnityEngine;
using UnityEngine.UI;

interface ICursorIconShower
{
    public int Locks { get; set; }
}

public class CarCursorIcon : MonoBehaviour, ICursorIconShower
{
    [Serializable]
    struct CursorModeConfig
    {
        public IPrize.PrizeApplyMode ApplyMode;
        public float AngleMult;
    }

    [SerializeField]
    private CursorModeConfig[] m_cursorConfigs;

    [field: SerializeField]
    public Sprite DirectionSprite { get; private set; }

    [field: SerializeField]
    public Sprite PointSprite { get; private set; }

    Canvas m_canvas;

    PrizeHost m_host;

    PlayerMousePrizeController m_prizeController;

    RectTransform m_transform;

    Camera m_camera;

    void Awake()
    {
        m_canvas = GetComponentInParent<Canvas>();
        m_camera = GetComponentInParent<Camera>();

        m_prizeController = GetComponentInParent<PlayerMousePrizeController>();
        m_host = GetComponentInParent<PrizeHost>();
        m_transform = (RectTransform)transform;
    }

    void OnDisable()
    {
        Cursor.visible = true;
    }

    public int Locks { get; set; }

    CursorModeConfig FindModeFor(IPrize.PrizeApplyMode mode)
    {
        for (int x = 0; x < m_cursorConfigs.Length; ++x)
        {
            if (m_cursorConfigs[x].ApplyMode == mode)
            {
                return m_cursorConfigs[x];
            }
        }

        return default;
    }

    void UpdateCursor(Sprite icon, float degressAngle, bool show)
    {
        for (int x = 0; x < m_transform.childCount; ++x)
        {
            RectTransform child = (RectTransform)m_transform.GetChild(x);
            child.eulerAngles = new(0, 0, degressAngle);

            child.GetComponent<Image>().sprite = icon;
            child.gameObject.SetActive(show);
        }
    }

    void Update()
    {
        if (Locks > 0)
        {
            Cursor.visible = true;
            UpdateCursor(null, 0f, false);
            return;
        }

        Cursor.visible = false;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_transform.parent, Input.mousePosition, m_camera, out var localPoint);
        m_transform.anchoredPosition = localPoint;

        if (m_prizeController && m_host.MainPrizeAccessed)
        {
            var config = FindModeFor(m_host.MainPrize.ApplyMode);
            UpdateCursor(
                m_prizeController.CursorDistanceIsShort ? PointSprite : DirectionSprite,
                Mathf.Atan2(-m_prizeController.DirectionOfCursor.x, m_prizeController.DirectionOfCursor.y) * Mathf.Rad2Deg * config.AngleMult,
                !m_host.PreviewEnabled || m_prizeController.PreviewIsInfinity
            );
        }
        else
        {
            UpdateCursor(PointSprite, 0f, true);
        }
    }
}
