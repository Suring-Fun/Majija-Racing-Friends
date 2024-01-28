using System.Collections;
using UnityEngine;

public class GameWinCondition : MonoBehaviour
{
    public (int my, int total)? Place { get; private set; }

    public string PlayerTag = "Player";

    [field: SerializeField]
    public int TargetLaps { get; private set; } = 5;


    private RoadPositionTracker m_trck;

    public RoadPositionTracker PlayerTracker => m_trck;

    public GameObject GameOverOverlay;

    public GameObject GameWonOverlay;

    public float WinScreenTime = 4f;

    public string DefaultSceneToGo = "Menu";

    public void Awake()
    {
        m_trck = GameObject.FindWithTag(PlayerTag).GetComponent<RoadPositionTracker>();
    }

    private void LateUpdate()
    {
        if (m_trck.Lap > TargetLaps && !Place.HasValue)
        {
            Place = m_trck.CalculatePlace();

            m_trck.GetComponentInChildren<PlayerControllerSelectionManager>().MakeAIControllable();
            if (Place.Value.my > 0)
            {
                GameOverOverlay.SetActive(true);
            }
            else
            {
                GameWonOverlay.SetActive(true);
                var info = FindObjectOfType<StageInfoHolder>().StageInfo;

                var progress = PlayerProgress.Main;
                int level = Mathf.Max(info.LevelRequired + 1, progress.PlayerLevel);

                if (progress.PlayerLevel != level)
                {
                    progress.PlayerLevel = level;
                    progress.SaveChanges();
                }

                IEnumerator Coro()
                {
                    yield return new WaitForSeconds(WinScreenTime);
                    SceneTransitionManager.Main.LaunchSceneTransition(DefaultSceneToGo);
                }

                StartCoroutine(Coro());
            }
        }
    }
}
