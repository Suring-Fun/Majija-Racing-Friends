internal class RAMSaveStorage : IProgressStorage
{
    private SerializeableProgressStorageData m_data = new();
    private SerializeableProgressStorageData m_savedEmulation = new();

    public IProgressStorage.IData Data => m_data;

    // We're always inited.
    public bool IsInited => true;

    public bool CheckIfThisObjectIsAppropriateForCurrentPlatform()
    {
        // We can work with each platform.
        return true;
    }

    public void Init()
    {
        // We need nothing to init.
    }

    public void Load()
    {
        // Nothing to load... Emulate loading from default file.
        m_data = m_savedEmulation.Clone();
    }

    public void Save()
    {
        // Nothing to save. Emulate it.
        m_savedEmulation = m_data.Clone();
    }
}