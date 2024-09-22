using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileSaveStorage : MonoBehaviour, IProgressStorage
{
    const string ProgressFileName = "MajijaMemories.json";

    private string m_fullFilePath;

    private readonly SerializeableProgressStorageData m_data = new();

    public IProgressStorage.IData Data => m_data;

    public bool IsInited { get; private set; }

    public bool CheckIfThisObjectIsAppropriateForCurrentPlatform()
    {
#if UNITY_WEBGL
        return false; // Web browsers has no access to filesystem.
#else
        return true;
#endif
    }

    public void Init()
    {
        m_fullFilePath = Path.Combine(Application.persistentDataPath, ProgressFileName);
        IsInited = true;
    }

    public void Load()
    {
        if (File.Exists(m_fullFilePath))
        {
            JsonUtility.FromJsonOverwrite(
                File.ReadAllText(m_fullFilePath, Encoding.UTF8),
                m_data
                );
            Debug.Log($"Loaded: {m_fullFilePath}");
        }
        else
            Debug.Log($"Nothing to load: {m_fullFilePath}");
    }

    public void Save()
    {
        Debug.Log($"Saved: {m_fullFilePath}");
        File.WriteAllText(
            m_fullFilePath,
            JsonUtility.ToJson(m_data),
            Encoding.UTF8
            );
    }
}
