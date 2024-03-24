using UnityEngine;

public class PlayerMousePrizeController : MonoBehaviour
{
    Transform m_car;

    PrizeHost m_prizeHost;

    Camera m_camera;

    private void Awake()
    {
        m_prizeHost = GetComponentInParent<PrizeHost>();
        m_car = m_prizeHost.transform;

        m_camera = m_prizeHost.GetComponentInChildren<Camera>();
    }

    private Vector2 m_lastSavedDirection;

    private bool m_actionPlanned;

    [field: SerializeField]
    public float MinDistance = 2f;

    private void Update()
    {
        if (Input.GetMouseButton(0) && m_prizeHost.MainPrizeAccessed)
        {
            m_prizeHost.MainPrizeIsLocked = true;
            m_actionPlanned = true;

            Vector3 worldDir = m_camera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward) - m_car.position;
            Vector2 direction = new Vector2(
                Vector2.Dot(transform.right, worldDir),
                Vector2.Dot(transform.up, worldDir)
            );

            if (direction.sqrMagnitude < MinDistance * MinDistance)
            {
                m_prizeHost.DisableApplyView();
                m_actionPlanned = false;
                return;
            }

            if (m_prizeHost.MainPrize.ApplyMode == IPrize.PrizeApplyMode.JustApply)
            {
                m_lastSavedDirection = Vector2.up;
                m_prizeHost.EnableApplyPreview(Vector2.up);
                return;
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
            if (m_actionPlanned && m_prizeHost.MainPrizeAccessed && Input.GetMouseButtonUp(0))
                m_prizeHost.ApplyPrize(m_lastSavedDirection);

            m_prizeHost.DisableApplyView();
            m_prizeHost.MainPrizeIsLocked = false;
        }
    }
}
