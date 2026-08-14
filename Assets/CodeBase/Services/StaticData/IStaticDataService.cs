using CodeBase.StaticData.Audio;
using CodeBase.StaticData.Player;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Window;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();

        WindowConfig ForWindow(WindowId id);
        AudioConfig ForAudio(AudioConfigId configId);
        PlayerStaticData PlayerData();
    }
}