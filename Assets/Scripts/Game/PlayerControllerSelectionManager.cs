using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerSelectionManager : MonoBehaviour
{
    public GameObject AIVer;
    public GameObject StandaloneVer;
    public GameObject MobileVer;

    private GameObject m_current;

    [ContextMenu("Make AI controllable")]
    public void MakeAIControllable() {
        Destroy(m_current);
        m_current = Instantiate(AIVer, transform, false);
    }

    public void Awake() {
        GameObject prefab = StandaloneVer;
        #if UNITY_EDITOR
        prefab = StandaloneVer;
        #elif UNITY_ANDROID
        prefab = MobileVer;
        #endif

        Debug.LogWarning("Select control based on platform");

        m_current = Instantiate(prefab, transform, false);
    }
}
