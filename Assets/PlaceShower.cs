using UnityEngine;
using UnityEngine.UI;

public class PlaceShower : MonoBehaviour
{
    public bool ShowLap;

    public string Format = "{0}/{1}";

    public string FinishTxt = "Finish";

    // Start is called before the first frame update
    void Start()
    {
        m_lab = GetComponent<Text>();
        m_gm = FindObjectOfType<GameWinCondition>();
    }

    int m_lastCell = -1;
    private Text m_lab;
    private GameWinCondition m_gm;
    public RoadPositionTracker m_trk;

    // Update is called once per frame
    void LateUpdate()
    {
        int cell = (int)(Time.time / 0.15f);

        if (cell != m_lastCell)
        {
            if (ShowLap)
            {
                if (m_gm.Place.HasValue)
                    m_lab.text = FinishTxt;
                else
                    m_lab.text = string.Format(Format, m_trk.Lap, m_gm.TargetLaps);
            }
            else
            {
                var place = m_gm.Place ?? m_trk.CalculatePlace();
                m_lab.text = string.Format(Format, place.my + 1, place.total);
            }
        }

        m_lastCell = cell;
    }
}
