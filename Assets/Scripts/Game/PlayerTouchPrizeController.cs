using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTouchPrizeController : MonoBehaviour
{
    [Serializable]
    public class StickProfile
    {
        [field: SerializeField]
        public IPrize.PrizeApplyMode ModeApplyTo { get; private set; }

        [field: SerializeField]
        public Vector2 StickMult { get; private set; }

        [field: SerializeField]
        public Rect StickClamp { get; private set; } = new Rect(-1f, -1f, 2f, 2f);

        [field: SerializeField]
        public GameObject Substrate { get; private set; }


    }


    [field: SerializeField]
    public GameObject Stick { get; private set; }

    [field: SerializeField]
    public GameObject StickRoot { get; private set; }

    [field: SerializeField]
    public GameObject StickSubstrateRoot { get; private set; }

    [field: SerializeField]
    public float StickRadius { get; private set; } = 2f;

    [field: SerializeField]
    public GameObject IconRoot { get; private set; }

    [field: SerializeField]
    public float ThresholdBeforeUse { get; private set; } = 0.25f;

    [field: SerializeField]
    private StickProfile[] m_stickProfiles;

    private PrizeHost m_host;

    private GameObject m_lastSubstrate;

    private StickFetch m_stickFetch;

    private Vector2? m_applyVector;

    private void Awake()
    {
        m_host = GetComponentInParent<PrizeHost>();
        m_stickFetch = StickRoot.AddComponent<StickFetch>();
        m_stickFetch.Radius = StickRadius;

    }

    private void OnDisable() {
        ShowIcon();
    }

    private StickProfile FindStickProfileFor(IPrize.PrizeApplyMode mode)
    {
        for (int x = 0; x < m_stickProfiles.Length; ++x)
        {
            var profile = m_stickProfiles[x];
            if (profile.ModeApplyTo == mode)
            {
                return profile;
            }
        }

        return default;
    }

    private void ConfigureStick(StickProfile profile)
    {
        m_stickFetch.Mult = profile.StickMult;
        m_stickFetch.Clamp = profile.StickClamp;
        if (m_lastSubstrate != profile.Substrate)
        {
            if (m_lastSubstrate)
                m_lastSubstrate.SetActive(false);

            m_lastSubstrate = profile.Substrate;
        }
        profile.Substrate.SetActive(true);
    }

    private void ShowIcon()
    {
        IconRoot.SetActive(true);
        StickSubstrateRoot.SetActive(false);
    }

    private void ShowSubstrate()
    {
        IconRoot.SetActive(false);
        StickSubstrateRoot.SetActive(true);
        ConfigureStick(FindStickProfileFor(m_host.MainPrize.ApplyMode));
    }

    private void UpdateStickPosition()
    {
        Stick.transform.localPosition = m_stickFetch.Position * StickRadius;
    }

    void UpdateApplyPreview()
    {
        if (m_stickFetch.Position.magnitude >= ThresholdBeforeUse)
        {
            m_host.EnableApplyPreview(m_stickFetch.Position);
        }
        else
        {
            m_host.DisableApplyView();
        }
    }

    void UpdateApplyVector()
    {
        if (m_stickFetch.Position.magnitude >= ThresholdBeforeUse)
        {
            m_applyVector = m_stickFetch.Position;
        }
        else
        {
            m_applyVector = null;
        }
    }

    void LateUpdate()
    {
        var prize = m_host.MainPrize;

        if (prize is object)
        {
            bool iconModeIsEnabled = !m_stickFetch.IsInUse | !m_host.MainPrizeAccessed;

            if (iconModeIsEnabled)
            {
                ShowIcon();
                if (m_applyVector.HasValue)
                {
                    m_host.ApplyPrize(m_applyVector.Value);
                    m_applyVector = null;
                }
                m_host.DisableApplyView();
                m_host.MainPrizeIsLocked = false;
            }
            else
            {
                ShowSubstrate();
                UpdateStickPosition();
                UpdateApplyVector();
                UpdateApplyPreview();
                m_host.MainPrizeIsLocked = true;
            }

        }
    }

}
