using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDpiShow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "My DPI: " + DeviceScreenInfo.MeasureDPI()?.ToString() ?? "Unknown"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
