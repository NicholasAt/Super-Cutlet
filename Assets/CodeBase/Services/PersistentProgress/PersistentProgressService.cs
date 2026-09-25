using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        [field: SerializeField] public PlayerProgress Progress { get; private set; }
        [field: SerializeField] public Settings Settings { get; private set; }

        public void SetPlayerProgress(PlayerProgress progress) =>
                Progress = progress;

        public Settings NewSettings() => 
            Settings = new Settings();

        public void SetSettings(Settings settings) =>
            Settings = settings;

        public PlayerProgress NewPlayerProgress() =>
            Progress = new PlayerProgress();
    }
}