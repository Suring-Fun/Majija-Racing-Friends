using UnityEngine;
using UnityEngine.UI;

public class StageSelectionManager : MonoBehaviour
{

    [field: SerializeField] private StageSelectButton[] m_buttons;

    [field: SerializeField] public Image StageCoverContainer { get; private set; }

    [field: SerializeField] public Text StageLabelContainer { get; private set; }
    [field: SerializeField] public Text StageLabelShadowContainer { get; private set; }


    [field: SerializeField] public Image CharContainer { get; private set; }

    public StageInfo Info { get; internal set; }
    
    [field: SerializeField]
    public float SecondsPerFrame = 1f / 12f;

    private StageSelectButton m_currentSelectedButton;

    void Awake()
    {
        foreach (var button in m_buttons)
        {
            button.Clicked += SelectButton;
            button.UpdateStatusWithLevel(PlayerProgress.Main.PlayerLevel);
        }

        SelectButton(m_buttons[Mathf.Clamp(PlayerProgress.Main.PlayerLevel, 0, m_buttons.Length - 1)]);
    }

    private Sprite[] m_currentCoverAnimation;

    // [field: SerializeField]

    private void UdpateInfo(StageInfo stageInfo)
    {
        Info = stageInfo;
        m_currentCoverAnimation = stageInfo.CoverAnimation;
        StageLabelContainer.text = stageInfo.StageName;
        StageLabelShadowContainer.text = stageInfo.StageName;

        
    }

    private void SelectButton(StageSelectButton s)
    {
        if (m_currentSelectedButton)
            m_currentSelectedButton.Selected = false;

        m_currentSelectedButton = s;

        if (s)
        {
            s.Selected = true;
            UdpateInfo(s.StageInfo);
        }

    }

    void Update() {
        Sprite[] anim = Info.SpritesOfCharacter;
        CharContainer.sprite = anim[(int)(Time.time / SecondsPerFrame) % anim.Length];

        StageCoverContainer.sprite = m_currentCoverAnimation[(int)(Time.time / SecondsPerFrame) % m_currentCoverAnimation.Length];
    }
}
