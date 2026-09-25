using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic
{
    public class ApplyAudioSettings : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private bool _isSFX;
        private IPersistentProgressService _progressService;

        public void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }
        private void Start()
        {
            if (_isSFX)
                _progressService.Settings.OnSFXChange += Refresh;
            else
                _progressService.Settings.OnMusicChange += Refresh;
            Refresh();
        }
        private void OnDestroy()
        {
            _progressService.Settings.OnSFXChange -= Refresh;
            _progressService.Settings.OnMusicChange -= Refresh;
        }
        private void Refresh()
        {
            _audioSource.volume = _isSFX ? _progressService.Settings.SFXVolume : _progressService.Settings.MusicVolume;
        }
    }
}