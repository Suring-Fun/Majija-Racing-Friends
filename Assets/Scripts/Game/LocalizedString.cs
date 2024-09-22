using UnityEngine;

[CreateAssetMenu(menuName = "LocalizedString", fileName = "LocalizedString")]
public class LocalizedString : ScriptableObject
{
    [field: SerializeField]
    [field: TextArea]
    public string DefaultValue { get; private set; } = string.Empty;

    [field: SerializeField]
    [field: TextArea]
    public string EnglishValue { get; private set; } = string.Empty;

    public override string ToString() 
        => PlatofrmUtility.GetLanguageIdentifer() == "ru" ? 
            DefaultValue : EnglishValue;


    public static implicit operator string(LocalizedString s) => s.ToString();
}
