using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.MainMenu
{
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField] private Slider _audioSlider;
        private ISaveLoadService _save;
        private IPersistentProgressService _persistentProgressService;

        public void Construct(ISaveLoadService saveLoadService, IPersistentProgressService persistentProgressService)
        {
            _save = saveLoadService;
            _persistentProgressService = persistentProgressService;
            _audioSlider.value = _persistentProgressService.Settings.AudioVolume;
            _audioSlider.onValueChanged.AddListener(ChangeVolume);
        }

        private void ChangeVolume(float value)
        {
            _persistentProgressService.Settings.SetAudioVolume(value);
            _save.SaveSettings();
        }
    }
}