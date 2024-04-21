using System;
using UnityEditor;
using UnityEngine;

public class PrizeHost : MonoBehaviour
{
    private IPrize m_mainPrize;


    public IPrize MainPrize
    {
        get => m_mainPrize;
        private set
        {
            m_mainPrize = value;
            MainPrizeChanged?.Invoke(this);
        }
    }

    public IPrize ReservedPrize { get; private set; }

    [field: SerializeField]
    public LineRenderer LineRenderer { get; private set; }

    [field: SerializeField]
    public Transform WhereAmIMark { get; private set; }

    public bool MainPrizeAccessed => m_mainPrize?.IsApplyable ?? false;

    public bool MainPrizeIsLocked
    {
        get => m_mainPrizeIsLocked;
        set
        {
            m_mainPrizeIsLocked = value;
        }
    }

    public event Action<PrizeHost> MainPrizeChanged;

    public void ApplyPrize(Vector2 direction)
        => MainPrize.Apply(direction);

    void OnTriggerStay2D(Collider2D c)
    {
        var prize = c.GetComponent<PrizeBody>();
        var p = prize.TryUse(transform);

        bool mainPrizeIsReplaceable = MainPrizeIsReplaceable;

        if (p is object)
        {
            if (mainPrizeIsReplaceable)
                MainPrize = p;
            else
                ReservedPrize = p;
        }
    }

    private bool MainPrizeIsReplaceable => !MainPrizeIsLocked && (MainPrize is null || MainPrize.IsReplaceable);

    void FixedUpdate()
    {
        if (MainPrize is object)
        {
            var result = MainPrize.Update(Time.deltaTime);
            if ((result & IPrize.UpdateResult.PrizeRetired) != 0)
            {
                MainPrize = ReservedPrize;
                ReservedPrize = null;
            }
        }

        if (ReservedPrize is object && MainPrizeIsReplaceable)
        {
            MainPrize = ReservedPrize;
            ReservedPrize = null;
        }
    }

    private Vector3[] m_poses = new Vector3[5];
    private bool m_mainPrizeIsLocked;

    [field: SerializeField]
    public float InfinityPrewiewDistance { get; private set; } = 10f;

    [field: SerializeField]
    public Gradient FiniteGradient { get; private set; }

    [field: SerializeField]
    public Gradient InfinityGradient { get; private set; }

    public bool PreviewIsInfinity { get; private set; } = false;

    public bool PreviewEnabled { get; private set; } = false;

    private void Awake()
    {
        LineRenderer.positionCount = m_poses.Length;
    }

    public void EnableApplyPreview(Vector2 direction)
    {
        PreviewEnabled = true;
        PreviewIsInfinity = false;
        Vector2 directionNormalized = direction.normalized;

        LineRenderer.enabled = true;

        float length = m_mainPrize.PreviewDistance(direction);

        if (float.IsInfinity(length))
        {
            length = InfinityPrewiewDistance;
            LineRenderer.colorGradient = InfinityGradient;
            WhereAmIMark.gameObject.SetActive(false);
            PreviewIsInfinity = true;
        }
        else
        {
            WhereAmIMark.gameObject.SetActive(true);
            WhereAmIMark.localPosition = (Vector3)(length * directionNormalized) + Vector3.forward * WhereAmIMark.localPosition.z;
            LineRenderer.colorGradient = FiniteGradient;
        }

        for (int x = 0; x < m_poses.Length; ++x)
        {
            m_poses[x] = directionNormalized * (length * x / (m_poses.Length - 1));
        }

        LineRenderer.SetPositions(m_poses);
    }

    public void DisableApplyView()
    {
        LineRenderer.enabled = false;
        WhereAmIMark.gameObject.SetActive(false);
        PreviewIsInfinity = false;
        PreviewEnabled = false;
    }
}
