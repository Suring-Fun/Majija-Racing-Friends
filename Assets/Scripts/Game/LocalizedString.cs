using UnityEngine;

[CreateAssetMenu(menuName = "LocalizedString", fileName = "LocalizedString")]
public class LocalizedString : ScriptableObject
{
    [field: SerializeField] 
    [field: TextArea]
    public string DefaultValue { get; private set; } = string.Empty;
    public override string ToString()
    {
        return DefaultValue;
    }


    public static implicit operator string(LocalizedString s) => s.ToString();
}
