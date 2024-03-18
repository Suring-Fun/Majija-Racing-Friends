using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PrizeIconUpdater : MonoBehaviour
{
    [Serializable]
    public class PrizeAppearenceBehaviour
    {
        [field: SerializeField]
        public int LayerIndex { get; private set; }

        [field: SerializeField]
        public string AppearingStateName { get; private set; }

        [field: SerializeField]
        public string ReusingStateName { get; private set; }

        [field: SerializeField]
        public string HidingStateName { get; private set; }

        [field: SerializeField]
        public string ReplacingStateName { get; private set; }

        [field: SerializeField]
        public string HiddenStateName { get; private set; }

        [field: SerializeField]
        public float ReplacingTime { get; private set; }

        private IPrize m_shownPrize;

        public IPrize LastShownPrize => m_shownPrize;

        private float m_time;

        [field: SerializeField]
        private Image[] m_iconImages;

        private void SetIcon(Sprite icon)
        {
            foreach (var i in m_iconImages)
                i.sprite = icon;
        }

        private void Appear(Animator animator)
            => animator.Play(AppearingStateName, LayerIndex, 0f);

        private void Disappear(Animator animator)
            => animator.Play(HidingStateName, LayerIndex, 0f);

        private void Reuse(Animator animator)
            => animator.Play(ReusingStateName, LayerIndex, 0f);


        private void Replace(Animator animator)
            => animator.Play(ReplacingStateName, LayerIndex, 0f);

        private void Hide(Animator animator)
            => animator.Play(HiddenStateName, LayerIndex, 0f);

        public void UpdateState(Animator animator, float delta, IPrize srcPrizeState, IPrize prize, IPrize dstPrizeState)
        {
            if (prize is object)
            {
                if (m_shownPrize is null)
                {
                    m_time = -1f;
                    Appear(animator);
                }
                else if (prize != m_shownPrize)
                {
                    if (srcPrizeState == prize)
                        m_time = ReplacingTime;
                    else
                    {
                        m_time = -1f;
                        Reuse(animator);
                    }
                }

                if (m_time <= 0f)
                    SetIcon(prize.Icon);
            }
            else if (m_shownPrize is object)
            {
                if (dstPrizeState == m_shownPrize)
                {
                    m_time = ReplacingTime;
                    Replace(animator);
                }
                else
                {
                    m_time = -1f;
                    if (m_shownPrize is object)
                        SetIcon(m_shownPrize.Icon);
                    Disappear(animator);
                }
            }

            m_shownPrize = prize;

            if (m_time > 0f)
            {
                m_time -= delta;
                if (m_time <= 0f && prize is null)
                {
                    Hide(animator);
                }
            }
        }
    }

    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public PrizeAppearenceBehaviour MainPrizeAppearenceBehaviour { get; private set; }

    [field: SerializeField]
    public PrizeAppearenceBehaviour ReservedPrizeAppearenceBehaviour { get; private set; }

    [field: SerializeField]
    private Image[] m_fillImages;

    private PrizeHost m_host;

    private void Awake()
    {
        m_host = GetComponentInParent<PrizeHost>();

    }

    private void UpdateFill(float fill01)
    {
        for (int x = 0; x < m_fillImages.Length; ++x)
        {
            var i = m_fillImages[x];
            i.fillAmount = fill01;
        }
    }


    void LateUpdate()
    {
        var prize = m_host.MainPrize;

        if (prize is object)
            UpdateFill(prize.Amount);

        var reservedPrize = m_host.ReservedPrize;
        MainPrizeAppearenceBehaviour.UpdateState(Animator, Time.deltaTime, ReservedPrizeAppearenceBehaviour.LastShownPrize, prize, null);
        ReservedPrizeAppearenceBehaviour.UpdateState(Animator, Time.deltaTime, null, reservedPrize, prize);
    }
}
