using UnityEngine;

[CreateAssetMenu(menuName = "StageInfo", fileName = "StageInfo")]
public class StageInfo : ScriptableObject
{
    [field: SerializeField] public Sprite[] CoverAnimation { get; private set; }

    [field: SerializeField] public LocalizedString StageName { get; private set; }

    [field: SerializeField] public string SceneName { get; private set; } = string.Empty;

    [field: SerializeField] public int LevelRequired { get; private set; } = 0;

    [field: SerializeField] public Sprite[] SpritesOfCharacter { get; private set; }
}
