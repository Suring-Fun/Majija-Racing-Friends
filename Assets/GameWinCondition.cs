using UnityEngine;

public class GameWinCondition : MonoBehaviour
{
    public (int my, int total)? Place { get; private set; }



    public string PlayerTag = "Player";

    public int TargetLaps { get; private set; } = 5;

    private RoadPositionTracker m_trck;

    public void Awake()
    {
        m_trck = GameObject.FindWithTag(PlayerTag).GetComponent<RoadPositionTracker>();
    }

    private void LateUpdate()
    {
        if(m_trck.Lap > TargetLaps && !Place.HasValue) {
            Place = m_trck.CalculatePlace();
        }
    }
}
