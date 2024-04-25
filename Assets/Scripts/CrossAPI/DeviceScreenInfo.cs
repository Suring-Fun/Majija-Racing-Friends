using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class DeviceScreenInfo
{

    [DllImport("__Internal")]
    private static extern float MeasureWebDPI();

    public static float? MeasureDPI()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            return MeasureWebDPI();
#else
        var editorDPI = Screen.dpi;
        return editorDPI == 0 ? null : editorDPI;
#endif
    }
}
