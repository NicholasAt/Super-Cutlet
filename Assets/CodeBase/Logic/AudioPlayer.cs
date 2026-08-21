using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Audio;
using UnityEngine;

namespace CodeBase.Logic
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private IStaticDataService _staticData;
        private IPersistentProgressService _persistentProgressService;

        public void Construct(IStaticDataService staticData, IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;
            _staticData = staticData;
            persistentProgressService.Settings.OnChangeAudioVolume += ChangeVolume;
            SetSettings();
        }

        private void ChangeVolume() =>
            _audioSource.volume = _persistentProgressService.Settings.AudioVolume;

        public void Play(AudioConfigId configId)
        {
            _audioSource.clip = _staticData.ForAudio(configId).Clip;
            _audioSource.Play();
        }

        private void SetSettings()
        {
            ChangeVolume();
            _audioSource.playOnAwake = false;
            _audioSource.loop = true;
        }
    }
}