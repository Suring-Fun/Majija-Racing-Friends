using UnityEngine;
using UnityEngine.UI;

public class SimpleTextLocalizer : MonoBehaviour
{
    public LocalizedString LocalizedString;

    void Start()
    {
        GetComponent<Text>().text = LocalizedString;
    }

}
