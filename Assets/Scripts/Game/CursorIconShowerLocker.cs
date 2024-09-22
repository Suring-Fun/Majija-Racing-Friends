using UnityEngine;

public class CursorIconShowerLocker : MonoBehaviour
{
    ICursorIconShower m_cursorShower;
    void Awake()
    {
        var roots = gameObject.scene.GetRootGameObjects();
        foreach (var root in roots)
        {
            var shower = root.GetComponentInChildren<ICursorIconShower>();
            if (shower != null)
            {
                m_cursorShower = shower;
                return;
            }
        }
    }

    void OnEnable()
    {
        if (m_cursorShower != null)
            m_cursorShower.Locks++;
    }
    void OnDisable()
    {
        if (m_cursorShower != null)
            m_cursorShower.Locks--;
    }
}
