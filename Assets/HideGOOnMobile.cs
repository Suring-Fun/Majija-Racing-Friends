using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class HideGOOnMobile : MonoBehaviour
{
    void Start()
    {
        if (YandexGame.EnvironmentData.isMobile)
            gameObject.SetActive(false);
    }

}
