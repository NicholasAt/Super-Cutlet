using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.MainMenu
{
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField] private Slider _audioSlider;
        [SerializeField] private bool _isSFX;

        private ISaveLoadService _save;
        private IPersistentProgressService _persistentProgressService;

        public void Construct(ISaveLoadService saveLoadService, IPersistentProgressService persistentProgressService)
        {
            _save = saveLoadService;
            _persistentProgressService = persistentProgressService;

        }
        private void Start()
        {
            Settings settings = GetSettings();
            _audioSlider.value = _isSFX ? settings.SFXVolume : settings.MusicVolume;
            _audioSlider.onValueChanged.AddListener(ChangeVolume);
        }

        private void ChangeVolume(float value)
        {
            Settings settings = GetSettings();
            if (_isSFX)
                settings.SetSFXVolume(value);
            else
                settings.SetMusicVolume(value);
            _save.SaveSettings();
        }
        private Settings GetSettings()
        {
            return _persistentProgressService.Settings;
        }
    }
}