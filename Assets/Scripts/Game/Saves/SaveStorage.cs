using UnityEngine;

/// <summary>
/// Controls game variables to be saved.
/// </summary>
/// <remarks>It's just proxies result to underlyed object.</remarks>
public class SaveStorage : MonoBehaviour
{
    private static IProgressStorage m_underlyingObject;

    public static IProgressStorage.IData Data => m_underlyingObject.Data;

    public static bool IsInited => m_underlyingObject?.IsInited ?? false;

    public static void Save() => m_underlyingObject.Save();
    
    public static void Load() => m_underlyingObject.Load(); 

    void Awake()
    {
        // Searching for appropriate underlying object.
        var storageImplementations = GetComponentsInChildren<IProgressStorage>();

        // Searching for the first appropriate.
        foreach (var implementation in storageImplementations)
        {
            bool implementationIsAppropriateForCurrentPlatform =
                implementation.CheckIfThisObjectIsAppropriateForCurrentPlatform();

            if (implementationIsAppropriateForCurrentPlatform)
            {

                // Init the platform (eg. create plugin prefab, etc.)
                implementation.Init();
                m_underlyingObject = implementation;

                return;
            }
        }

        Debug.LogWarning($"No appropriate {nameof(IProgressStorage)} found! We're working without saving feature!");

        // Create dummy save object.
        m_underlyingObject = new RAMSaveStorage();
        m_underlyingObject.Init();
    }
}
