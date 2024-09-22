using UnityEngine;

public class HideGOOnMobile : MonoBehaviour
{
    void Start()
    {
        if (PlatofrmUtility.CheckIfIsAnTablet())
            gameObject.SetActive(false);
    }

}
