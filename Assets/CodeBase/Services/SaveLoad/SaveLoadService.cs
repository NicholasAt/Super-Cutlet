using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string PlayerProgressPrefsKey = "ProgressSuperCatlet2D";
        private const string SettingsPrefsKey = "SettingsSuperCatlet2D";

        private readonly IPersistentProgressService _persistentProgress;

        public SaveLoadService(IPersistentProgressService persistentProgress)
        {
            _persistentProgress = persistentProgress;
        }

        public void ClearPlayerProgress() => 
            PlayerPrefs.DeleteKey(PlayerProgressPrefsKey);

        public void SavePlayerProgress() =>
            WriteJson(PlayerProgressPrefsKey, _persistentProgress.Progress);

        public void SaveSettings() =>
            WriteJson(SettingsPrefsKey, _persistentProgress.Settings);

        public PlayerProgress LoadPlayerProgress() =>
            ReadJson<PlayerProgress>(PlayerProgressPrefsKey);

        public Settings LoadSettings() =>
            ReadJson<Settings>(SettingsPrefsKey);

        private TObj ReadJson<TObj>(string path) =>
            JsonUtility.FromJson<TObj>(PlayerPrefs.GetString(path));

        private void WriteJson(string path, object obj) =>
            PlayerPrefs.SetString(path, JsonUtility.ToJson(obj));
    }
}