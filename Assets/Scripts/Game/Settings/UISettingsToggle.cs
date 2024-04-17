using UnityEngine;
using UnityEngine.UI;

public class UISettingsToggle : MonoBehaviour
{
    private Toggle m_toggle;
    
    [field: SerializeField]
    public string SettingName { get; private set; }
    
    void Awake() {
        m_toggle = GetComponent<Toggle>();
    }

    void Start() {
        m_toggle.isOn = PlayerSettings.Instance[SettingName];
        m_toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool value)
    {
        PlayerSettings.Instance[SettingName] = value;
    }
}
