using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGOWithSetting : MonoBehaviour
{

    public string Setting;

    void Start()
    {
        gameObject.SetActive(PlayerSettings.Instance[Setting]);   
    }
 }
