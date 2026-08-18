using CodeBase.Data;

namespace CodeBase.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void SavePlayerProgress();
        PlayerProgress LoadPlayerProgress();
        void SaveSettings();
        Settings LoadSettings();
        void ClearPlayerProgress();
    }
}