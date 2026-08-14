using CodeBase.StaticData.Audio;
using CodeBase.StaticData.Player;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Window;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string WindowStaticDataPath = "StaticData/WindowStaticData";
        private const string AudioStaticDataPath = "StaticData/AudioData";
        private const string PlayerStaticDataPath = "StaticData/PlayerData";

        private Dictionary<AudioConfigId, AudioConfig> _audioConfigs;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;
        private PlayerStaticData _player;

        public void Load()
        {
            _windowConfigs = Resources.Load<WindowStaticData>(WindowStaticDataPath).Configs.ToDictionary(x => x.WindowId, x => x);
            _audioConfigs = Resources.Load<AudioStaticData>(AudioStaticDataPath).Configs.ToDictionary(x => x.ConfigId, x => x);
            _player = Resources.Load<PlayerStaticData>(PlayerStaticDataPath);
        }

        public PlayerStaticData PlayerData() =>
            _player;

        public AudioConfig ForAudio(AudioConfigId configId) =>
            _audioConfigs.TryGetValue(configId, out var data) ? data : null;

        public WindowConfig ForWindow(WindowId id) =>
            _windowConfigs.TryGetValue(id, out var data) ? data : null;
    }
}