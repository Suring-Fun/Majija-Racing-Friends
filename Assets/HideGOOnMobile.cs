using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public static class PlatofrmUtility { 
    public static bool CheckIfIsAnTablet() {
        return !YandexGame.EnvironmentData.isDesktop;
    }
}

public class HideGOOnMobile : MonoBehaviour
{
    void Start()
    {
        if (PlatofrmUtility.CheckIfIsAnTablet())
            gameObject.SetActive(false);
    }

}
