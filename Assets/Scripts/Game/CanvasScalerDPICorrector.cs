using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerDPICorrector : MonoBehaviour
{
    public float FallbackDPI = 72f;

    public float Scale = 1.5f;

    const float CentimetresPerInch = 2.54f;

    const float InchesPerCentimetre = 1f / CentimetresPerInch;

    void Awake()
    {
        var scaler = GetComponent<CanvasScaler>();

        if (DeviceScreenInfo.MeasureDPI() is float dpi)
            scaler.scaleFactor = dpi * InchesPerCentimetre * Scale;
        else
            scaler.scaleFactor = FallbackDPI * InchesPerCentimetre * Scale;
    }
}
