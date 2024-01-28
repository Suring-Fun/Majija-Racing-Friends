using UnityEngine;

public class ByEdgeDestroyable : MonoBehaviour
{
    private PathData m_pd;

    public bool DestroyOnlyIfEdgeIsSolid;

    void Awake()
    {
        m_pd = FindObjectOfType<PathData>();

    }

    void FixedUpdate()
    {
        if (!DestroyOnlyIfEdgeIsSolid || m_pd.EdgesAreSolid)
        {
            (var dist, _, var rad, _) = m_pd.GetLocationAtTrack(transform.position);

            if (dist > rad + m_pd.DistanceToWater + m_pd.DistanceToDeepWater)
                Destroy(gameObject);

        }
    }
}
