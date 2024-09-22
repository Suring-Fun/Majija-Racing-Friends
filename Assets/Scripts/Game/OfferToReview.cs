using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfferToReview : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Rewiew);
    }

    private void Rewiew()
    {
        Debug.Log("Review dialouge offered");
    }
}
