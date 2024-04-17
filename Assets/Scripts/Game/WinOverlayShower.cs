using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinOverlayShower : MonoBehaviour
{
    public GameObject Overlay;

    public int Threshold = 4;

    void Start()
    {
        if (PlayerProgress.Main.PlayerLevel >= Threshold)
            Overlay.SetActive(true);
    }
}
