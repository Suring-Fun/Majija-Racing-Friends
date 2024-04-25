using UnityEngine;

public class GraphicsBlinker : MonoBehaviour
{
    private bool m_isShown = true;

    public bool IsShown
    {
        get => m_isShown; set => ChangeStatus(value);
    }

    private SpriteRenderer[] m_spriteRenderers;

    void Awake()
    {
        m_spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void ChangeStatus(bool isShown)
    {
        if (isShown != m_isShown)
        {
            for (int x = 0; x < m_spriteRenderers.Length; ++x)
            {
                m_spriteRenderers[x].enabled = isShown;
            }
            m_isShown = isShown;
        }
    }
}
