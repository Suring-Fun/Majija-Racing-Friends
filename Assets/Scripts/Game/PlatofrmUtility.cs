using UnityEngine;

public static class PlatofrmUtility {
    public static bool CheckIfIsAnTablet()
    {
        // TODO: Use Plugin's data.
        #if UNITY_EDITOR
        return false;
        #else
        return Application.isMobilePlatform;
        #endif
    }

    public static string GetLanguageIdentifer() 
        => System.Globalization.CultureInfo.
            CurrentCulture.TwoLetterISOLanguageName;
}
