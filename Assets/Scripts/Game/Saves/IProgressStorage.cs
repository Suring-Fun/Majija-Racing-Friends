public interface IProgressStorage
{
    public interface IData
    {
        public int CurrentPlayerLevel { get; set; }
        bool EnableMusic { get; set; }
        bool EnableSound { get; set; }
        bool EnableHelp { get; set; }
    }

    bool IsInited { get; }
    IData Data { get; }
    bool CheckIfThisObjectIsAppropriateForCurrentPlatform();
    void Init();
    void Save();
    void Load();
}
