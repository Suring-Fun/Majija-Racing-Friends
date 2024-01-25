using UnityEngine;
using UnityEngine.UI;

public class PrizeIconUpdater : MonoBehaviour
{
    [field: SerializeField]
    public GameObject Root { get; private set; }


    [field: SerializeField]
    private Image[] m_iconImages;

    [field: SerializeField]
    private Image[] m_fillImages;

    private PrizeHost m_host;

    private void Awake()
    {
        m_host = GetComponent<PrizeHost>();

    }

    private void UpdateFill(float fill01)
    {
        for (int x = 0; x < m_fillImages.Length; ++x)
        {
            var i = m_fillImages[x];
            i.fillAmount = fill01;
        }
    }

    private void UpdateIcon(Sprite sprite)
    {
        for (int x = 0; x < m_iconImages.Length; ++x)
        {
            var i = m_iconImages[x];
            i.sprite = sprite;
        }
    }



    void LateUpdate()
    {
        var prize = m_host.Prize;

        if (prize is object)
        {
            Root.SetActive(true);
            UpdateFill(prize.Amount);
            UpdateIcon(prize.Icon);
        }
        else
        {
            Root.SetActive(false);
        }
    }
}
