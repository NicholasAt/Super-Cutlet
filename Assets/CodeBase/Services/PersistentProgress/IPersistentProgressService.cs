using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; }
        Settings Settings { get; }
        PlayerProgress NewPlayerProgress();
        void SetPlayerProgress(PlayerProgress progress);
        Settings NewSettings(float audioVolume);
        void SetSettings(Settings settings);
    }
}