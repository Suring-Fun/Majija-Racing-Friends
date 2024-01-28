using System;
using UnityEngine;
using UnityEngine.UI;

public class CounterUpdater : MonoBehaviour
{
    private Text m_label;

    private void Start()
    {
        m_label = GetComponent<Text>();
        FindObjectOfType<GameStartCondition>().LabelChanged += LabelChanged;
    }

    private void LabelChanged(string label, bool isHidden)
    {
        m_label.enabled = !isHidden;
        m_label.text = label;
    }
}
