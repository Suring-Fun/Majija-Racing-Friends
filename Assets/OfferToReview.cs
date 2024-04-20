using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class OfferToReview : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Rewiew);
    }

    private void Rewiew()
    {
        if (YandexGame.EnvironmentData.reviewCanShow)
        {
            YandexGame.ReviewShow(false);
        }
    }
}
