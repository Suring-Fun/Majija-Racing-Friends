using System;
using UnityEngine;

public class PrizeHost : MonoBehaviour
{
    private IPrize m_prize;

    public IPrize Prize
    {
        get => m_prize;
        private set
        {
            m_prize = value;
            PrizeChanged?.Invoke(this);
        }
    }

    [field: SerializeField]
    public LineRenderer LineRenderer { get; private set; }

    [field: SerializeField]
    public Transform WhereAmIMark { get; private set; }

    public bool PrizeAccessed => m_prize?.IsApplyable ?? false;

    public event Action<PrizeHost> PrizeChanged;

    public void ApplyPrize(Vector2 direction)
        => Prize.Apply(direction);

    void OnTriggerStay2D(Collider2D c)
    {
        var prize = c.GetComponent<PrizeBody>();
        var p = prize.TryUse(transform);

        if (Prize is null && p is object)
            Prize = p;
    }


    void FixedUpdate()
    {
        if (Prize is object)
        {
            var result = Prize.Update(Time.deltaTime);
            if ((result & IPrize.UpdateResult.PrizeRetired) != 0)
                Prize = null;
        }
    }

    private Vector3[] m_poses = new Vector3[5];

    [field: SerializeField]
    public float InfinityPrewiewDistance { get; private set; } = 10f;

    [field: SerializeField]
    public Gradient FiniteGradient { get; private set; }

    [field: SerializeField]
    public Gradient InfinityGradient { get; private set; }

    private void Awake()
    {
        LineRenderer.positionCount = m_poses.Length;
    }

    public void EnableApplyPreview(Vector2 direction)
    {
        Vector2 directionNormalized = direction.normalized;
        
        LineRenderer.enabled = true;

        float length = m_prize.PreviewDistance(direction);

        if (float.IsInfinity(length))
        {
            length = InfinityPrewiewDistance;
            LineRenderer.colorGradient = InfinityGradient;
            WhereAmIMark.gameObject.SetActive(false);
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
    }
}
