using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Audio;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private IStaticDataService _staticDataService;
        private IPersistentProgressService _persistentProgressService;

        public void Construct(IStaticDataService staticDataService, IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
        }
        private void Start()
        {
            _persistentProgressService.Settings.OnSFXChange += Refresh;
            Refresh();
        }
        private void OnDestroy()
        {
            _persistentProgressService.Settings.OnSFXChange -= Refresh;
        }
        private void Refresh() =>
            _audioSource.volume = _persistentProgressService.Settings.SFXVolume;

        public void Play(AudioConfigId configId) =>
            _audioSource.PlayOneShot(_staticDataService.ForAudio(configId).Clip);
    }
}