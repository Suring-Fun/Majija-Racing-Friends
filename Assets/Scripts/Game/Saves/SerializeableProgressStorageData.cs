using UnityEngine;

[System.Serializable]
class SerializeableProgressStorageData : IProgressStorage.IData
{
    [field: SerializeField]
    public int CurrentPlayerLevel { get; set; } = -1; // = no game started

    [field: SerializeField]
    public bool EnableMusic { get; set; } = true;

    [field: SerializeField]
    public bool EnableSound { get; set; } = true;

    [field: SerializeField]
    public bool EnableHelp { get; set; } = true;

    public SerializeableProgressStorageData Clone()
    {
        // TODO: Implement fast way to clone object. Don't do this mess.
        return JsonUtility.FromJson<SerializeableProgressStorageData>(
            JsonUtility.ToJson(this)
        );
    }
}
